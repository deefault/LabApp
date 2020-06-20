using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

// ReSharper disable once CheckNamespace
namespace LabApp.Server.Services.TeacherServices
{
	public class StudentAssignmentService
	{
		private readonly AppDbContext _db;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;


		public StudentAssignmentService(AppDbContext db, IUserService userService, IMapper mapper)
		{
			_db = db;
			_userService = userService;
			_mapper = mapper;
		}

		public async Task<StudentAssignment> GetByIdAsync(int id,
			params Expression<Func<StudentAssignment, object>>[] includes)
		{
			return await _db.StudentAssignment.AddIncludes(includes).SingleOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<StudentAssignment>> ListAsync(int? assignmentId, int? groupId = null,
			bool onlyNew = true,
			params Expression<Func<StudentAssignment, object>>[] includes)
		{
			// return await _db.StudentAssignment
			// 	.Include(x => x.Student)
			// 	.AddIncludes(includes)
			// 	.Where(x =>
			// 		x.Assignment.Subject.TeacherId == _userService.CurrentUserId &&
			// 		(
			// 			groupId == null ||
			// 			_db.StudentGroup.Where(y => y.GroupId == groupId).Select(y => y.StudentId).Contains(x.StudentId) //TODO user Assingment.GroupId
			// 		)
			// 	)
			// 	.ToListAsync();

			return await List(assignmentId, groupId, onlyNew, includes)
				.OrderBy(x => x.LastStatusChange)
				.ToListAsync();
		}

		private IQueryable<StudentAssignment> List(int? assignmentId, int? groupId = null, bool onlyNew = true,
			params Expression<Func<StudentAssignment, object>>[] includes) => _db.StudentAssignment
			.Include(x => x.Student)
			.AddIncludes(includes)
			.Where(x =>
				(
					assignmentId == null || x.AssignmentId == assignmentId
				) &&
				(
					groupId == null || x.GroupId == groupId
				) &&
				x.Assignment.TeacherId == _userService.UserId &&
				(
					onlyNew == false ||
					x.Status == AssignmentStatus.Submitted || x.Status == AssignmentStatus.NeedReview
				)
			);

		public class StudentWithAssignment
		{
			public Student Student { get; set; }

			public StudentAssignment Assignment { get; set; }
		}


		/// <summary>
		/// Для каждого студента группы найти его работу, либо null
		/// </summary>
		public async Task<IEnumerable<StudentWithAssignment>> ListAllGroupAsync(int assignmentId, int groupId)
		{
			return await _db.StudentGroup
				.Where(x => x.GroupId == groupId)
				.Select(x => new StudentWithAssignment
				{
					Student = x.Student,
					// ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
					Assignment = _db.StudentAssignment.Where(a =>
							a.AssignmentId == assignmentId && a.StudentId == x.StudentId
							                               && x.GroupId == groupId)
						.FirstOrDefault()
				})
				.ToListAsync();
		}

		/*public void UpdateStatus(StudentAssignment assignment, AssignmentStatus status)
		{
			assignment.Status = status;
			switch (status)
			{
				case AssignmentStatus.Submitted:
					assignment.Submitted = DateTime.UtcNow;
					break;
				case AssignmentStatus.ChangesRequested:
					// change LastStatusChange
					break;
				case AssignmentStatus.NeedReview:

				case AssignmentStatus.Approved:
					assignment.Completed = DateTime.UtcNow;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(status), status, null);
			}

			assignment.LastStatusChange = DateTime.UtcNow;
		}*/

		public void Approve(StudentAssignment assignment, decimal score, decimal? fineScore)
		{
			DateTime now = DateTime.UtcNow;
			assignment.Completed = now;
			assignment.LastStatusChange = now;
			assignment.Status = AssignmentStatus.Approved;

			UpdateScore(assignment, score, fineScore);
		}

		public void UpdateScore(StudentAssignment assignment, decimal score, decimal? fineScore)
		{
			decimal assignmentFine = assignment.Assignment.FineAfterDeadline;
			if (assignmentFine != 0)
			{
				// Расчитать штраф автоматом
				if (fineScore == null)
				{
					DateTime? deadLine = _db.GroupAssignment.FirstOrDefault(x =>
						x.AssignmentId == assignment.AssignmentId && x.GroupId == assignment.GroupId)?.DeadLine;
					if (deadLine != null && deadLine < assignment.Submitted)
					{
						fineScore = assignmentFine;
						score -= (decimal) fineScore;
					}
				}
				// Назначить другой штраф (возможно 0)
				else
				{
					fineScore = fineScore < 0 ? 0 : fineScore;
					score -= (decimal) fineScore;
				}
			}

			if (score < 0) score = 0;

			assignment.Score = score;
		}

		public void RequestChanges(StudentAssignment assignment)
		{
			assignment.Status = AssignmentStatus.ChangesRequested;
			assignment.LastStatusChange = DateTime.UtcNow;
		}

		public async Task<int> CountAsync(bool onlyNew)
		{
			return await List(null, null, onlyNew: onlyNew).CountAsync();
		}
	}
}