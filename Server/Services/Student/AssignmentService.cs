using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto.Student;
using LabApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

// ReSharper disable PossibleMultipleEnumeration

// ReSharper disable once CheckNamespace
namespace LabApp.Server.Services.StudentServices
{
	static class IQueryableExtensions
	{
		public static IQueryable<AssignmentWithConcrete> SelectWithStudentAssignment(this IQueryable<Assignment> source,
			int studentId)
		{
			return source.Select(x => new AssignmentWithConcrete
			{
				Assignment = x,
				StudentAssignment = x.Assignments.SingleOrDefault(y => y.StudentId == studentId)
			});
		}
	}

	public class AssignmentService
	{
		private readonly AppDbContext _db;
		private readonly GroupService _groupService;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;

		public AssignmentService(AppDbContext db, GroupService groupService, IMapper mapper, IUserService userService)
		{
			_db = db;
			_groupService = groupService;
			_mapper = mapper;
			_userService = userService;
		}


		public async Task<AssignmentWithConcrete> GetAssignmentById(int id, int studentId,
			params Expression<Func<Assignment, object>>[] includes)
		{
			return await _db.Assignments
				.Where(x => x.Id == id)
				.AddIncludes(includes)
				.SelectWithStudentAssignment(studentId)
				.SingleOrDefaultAsync();
		}

		public async Task<IEnumerable<AssignmentWithConcrete>> GetStudentAssignmentsAsync(int studentId)
		{
			var studentGroups = _groupService.Get(studentId, x => x.Group.Subject);
			var studentSubjects = studentGroups.Select(x => x.SubjectId);
			return await _db.Assignments
				.Where(x =>
					studentSubjects.Contains(x.SubjectId) &&
					!x.Groups.Any(g => studentSubjects.Contains(g.Group.SubjectId) && g.IsHidden)
				)
				.SelectWithStudentAssignment(studentId)
				.ToListAsync();
		}

		public async Task<IEnumerable<AssignmentWithConcrete>> GetStudentAssignmentsAsync(int studentId, Group group)
		{
			return await _db.Assignments
				.Where(x =>
					x.SubjectId == group.SubjectId &&
					!x.Groups.Any(ga => ga.GroupId == group.Id && ga.IsHidden)
				)
				.SelectWithStudentAssignment(studentId)
				.ToListAsync();
		}


		public async Task<bool> CheckUserAsync(Assignment assignment, int studentId)
		{
			return await _db.StudentGroup.AnyAsync(x =>
				x.StudentId == studentId && x.Group.SubjectId == assignment.SubjectId);
		}

		public async Task<StudentAssignment> GetStudentAssignmentById(int id)
		{
			return await _db.StudentAssignment.SingleOrDefaultAsync(x => x.Id == id);
		}

		public StudentAssignment Add(StudentAssignmentDto assignmentDto, User student, int assignmentId)
		{
			StudentAssignment studentAssignment = _mapper.Map<StudentAssignment>(assignmentDto);
			studentAssignment.StudentId = student.UserId;
			studentAssignment.Status = AssignmentStatus.Submitted;
			studentAssignment.LastStatusChange = DateTime.UtcNow;
			studentAssignment.Submitted = DateTime.UtcNow;
			studentAssignment.AssignmentId = assignmentId;

			var attachments = _db.StudentAssignmentAttachment
				.Where(x => assignmentDto.Attachments.Select(y => y.Id).Contains(x.Id) && x.UserId == student.UserId)
				.ToList();

			attachments.ForEach(x => x.Assignment = studentAssignment);
			_db.UpdateRange(attachments);
			return _db.Add(studentAssignment).Entity;
		}

		public bool IsAllowedToUpdate(StudentAssignment assignment) =>
			assignment.Status == AssignmentStatus.Submitted ||
			assignment.Status == AssignmentStatus.ChangesRequested ||
			assignment.Status == AssignmentStatus.NeedReview;

		public void NeedReview(StudentAssignment assignment)
		{
			assignment.Status = AssignmentStatus.NeedReview;
			assignment.LastStatusChange = DateTime.UtcNow;
		}
	}

	public class AssignmentWithConcrete
	{
		public Assignment Assignment { get; set; }

		[MaybeNull]
		public StudentAssignment StudentAssignment { get; set; }
	}
}