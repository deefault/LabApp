using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Server.Data.QueryModels;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto.Teacher;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;


// ReSharper disable once CheckNamespace
namespace LabApp.Server.Services.TeacherServices
{
	public class AssignmentService
	{
		private readonly AppDbContext _db;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public AssignmentService(AppDbContext db, IUserService userService, IMapper mapper)
		{
			_db = db;
			_userService = userService;
			_mapper = mapper;
		}

		public async Task<Assignment> GetByIdAsync(int id, params Expression<Func<Assignment, object>>[] includes)
		{
			return await _db.Assignments.AddIncludes(includes).SingleOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<Assignment>> ListAsync(int? subjectId = null,
			params Expression<Func<Assignment, object>>[] includes)
		{
			return await _db.Assignments
				.AddIncludes(includes)
				.Where(x => x.Subject.TeacherId == _userService.UserId
				            && (subjectId == null || x.SubjectId == subjectId))
				.ToListAsync();
		}

		public async Task<IEnumerable<Assignment>> ListByGroupAsync(Group group, params Expression<Func<Assignment, object>>[] includes)
		{
			return await _db.Assignments
				.AddIncludes(includes)
				.Where(x => x.Subject.TeacherId == _userService.UserId
				            && @group.SubjectId == x.SubjectId)
				.ToListAsync();
		}
		
	
		
		public async Task<List<AssignmentWithGroupInfo>> ListWithGroupInfoAsync(Group group, params Expression<Func<Assignment, object>>[] includes)
		{
			return await _db.Assignments
				.AddIncludes(includes)
				.Where(x => x.Subject.TeacherId == _userService.UserId && @group.SubjectId == x.SubjectId)
				.Select(x => new AssignmentWithGroupInfo
				{
					Assignment = x,
					// ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
					GroupAssignment = x.Groups.Where(y => y.GroupId == group.Id).FirstOrDefault()
				})
				.ToListAsync();
		}

		public async Task<Assignment> AddAsync(AssignmentDetailsDto assignmentDto, Subject subject)
		{
			Assignment assignment = (await _db.Assignments.AddAsync(_mapper.Map<Assignment>(assignmentDto))).Entity;
			assignment.TeacherId = _userService.UserId;
			assignment.Subject = subject;
			await _db.SaveChangesAsync();
			await _db.AssignmentAttachments
				.Where(x => assignmentDto.Attachments.Select(y => y.Id).Contains(x.Id) &&
				            x.UserId == _userService.UserId)
				.UpdateAsync(x => new AssignmentAttachment {AssignmentId = assignment.Id});
			await _db.Entry(assignment).Collection(x => x.Attachments).LoadAsync();

			return assignment;
		}

		public GroupAssignment GetGroupAssignment(int groupId, int assignmentId)
		{
			return _db.GroupAssignment.FirstOrDefault(x =>
				x.GroupId == groupId && x.AssignmentId == assignmentId && x.Group.TeacherId == _userService.UserId);
		}
	}
}