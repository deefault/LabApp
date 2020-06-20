using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Services.TeacherServices;
using LabApp.Shared.Dto;
using LabApp.Shared.Dto.Teacher;
using LabApp.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers.TeacherControllers
{
	public class StudentAssignmentsController : BaseTeacherController
	{
		private readonly AppDbContext _db;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly GroupService _groupService;
		private readonly AssignmentService _assignmentService;
		private readonly StudentAssignmentService _studentAssignmentService;
		private readonly AttachmentService _attachmentService;
		private readonly ConversationService _conversationService;

		public StudentAssignmentsController(AppDbContext db, IMapper mapper, IUserService userService,
			AssignmentService assignmentService, AttachmentService attachmentService, GroupService groupService,
			StudentAssignmentService studentAssignmentService, ConversationService conversationService)
		{
			_db = db;
			_mapper = mapper;
			_userService = userService;
			_assignmentService = assignmentService;
			_attachmentService = attachmentService;
			_groupService = groupService;
			_studentAssignmentService = studentAssignmentService;
			_conversationService = conversationService;
		}

		[HttpGet("/assignments/{assignmentId}/student-assignments/")]
		[ProducesResponseType(typeof(IEnumerable<StudentAssignmentDto>), 200)]
		public async Task<IActionResult> Get(int assignmentId, bool onlyNew = true)
		{
			IEnumerable<StudentAssignment> assignments =
				await _studentAssignmentService.ListAsync(assignmentId, null, onlyNew, x => x.Student,
					x => x.Assignment);

			return Ok(_mapper.Map<IEnumerable<StudentAssignmentDto>>(assignments));
		}

		[HttpGet("/groups/{groupId}/assignments/")]
		[ProducesResponseType(typeof(IEnumerable<StudentAssignmentDto>), 200)]
		public async Task<IActionResult> GetByGroup(int groupId, bool onlyNew = true)
		{
			IEnumerable<StudentAssignment> assignments =
				await _studentAssignmentService.ListAsync(null, groupId, onlyNew, x => x.Student, x => x.Assignment);

			return Ok(_mapper.Map<IEnumerable<StudentAssignmentDto>>(assignments));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<StudentAssignmentDto>), 200)]
		public async Task<IActionResult> GetAll(bool onlyNew = true)
		{
			IEnumerable<StudentAssignment> assignments =
				await _studentAssignmentService.ListAsync(null, null, onlyNew, x => x.Student, x => x.Assignment);

			return Ok(_mapper.Map<IEnumerable<StudentAssignmentDto>>(assignments));
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(StudentAssignmentDetailDto), 200)]
		public async Task<IActionResult> GetById(int id)
		{
			StudentAssignment assignment =
				await _studentAssignmentService.GetByIdAsync(id, x => x.Assignment, x => x.Attachments, x => x.Student);

			if (assignment == null) return NotFound();
			if (assignment.Assignment.TeacherId != _userService.UserId) return Forbid();

			return Ok(_mapper.Map<StudentAssignmentDetailDto>(assignment));
		}

		[HttpPatch("{id}/Approve")]
		[ProducesResponseType(typeof(StudentAssignmentDetailDto), 201)]
		public async Task<IActionResult> Approve(int id, decimal score, decimal? fineScore = null)
		{
			StudentAssignment assignment = await _studentAssignmentService.GetByIdAsync(id,
				x => x.Assignment, x => x.Attachments, x => x.Student);

			if (assignment == null) return NotFound();
			if (assignment.Assignment.TeacherId != _userService.UserId) return Forbid();

			if (assignment.Status != AssignmentStatus.NeedReview && assignment.Status != AssignmentStatus.Submitted)
				return BadRequest("Assignment status must be NeedReview or Submitted");

			_studentAssignmentService.Approve(assignment, score, fineScore);
			await _db.SaveChangesAsync();

			return CreatedAtAction(nameof(GetById), new {id}, _mapper.Map<StudentAssignmentDetailDto>(assignment));
		}

		[HttpPatch("{id}/RequestChanges")]
		[ProducesResponseType(typeof(StudentAssignmentDetailDto), 201)]
		public async Task<IActionResult> RequestChanges(int id)
		{
			StudentAssignment assignment = await _studentAssignmentService.GetByIdAsync(id,
				x => x.Assignment, x => x.Attachments, x => x.Student);

			if (assignment == null) return NotFound();
			if (assignment.Assignment.TeacherId != _userService.UserId) return Forbid();

			if (assignment.Status != AssignmentStatus.NeedReview && assignment.Status != AssignmentStatus.Submitted)
				return BadRequest("Assignment status must be NeedReview or Submitted");

			_studentAssignmentService.RequestChanges(assignment);
			await _db.SaveChangesAsync();

			return CreatedAtAction(nameof(GetById), new {id}, _mapper.Map<StudentAssignmentDetailDto>(assignment));
		}

		[HttpPatch("{id}")]
		[ProducesResponseType(typeof(StudentAssignmentDetailDto), 201)]
		public async Task<IActionResult> UpdateScore(int id, decimal score, decimal? fineScore = null)
		{
			StudentAssignment assignment = await _studentAssignmentService.GetByIdAsync(id,
				x => x.Assignment, x => x.Attachments, x => x.Student);

			if (assignment == null) return NotFound();
			if (assignment.Assignment.TeacherId != _userService.UserId) return Forbid();

			_studentAssignmentService.Approve(assignment, score, fineScore);
			await _db.SaveChangesAsync();

			return CreatedAtAction(nameof(GetById), new {id}, _mapper.Map<StudentAssignmentDetailDto>(assignment));
		}

		[HttpGet("{id}/conversation/")]
		[ProducesResponseType(typeof(ConversationDto), 200)]
		public async Task<IActionResult> GetConversation(int id)
		{
			StudentAssignment assignment = await _studentAssignmentService.GetByIdAsync(id,
				x => x.Assignment, x => x.Attachments, x => x.Student);
			if (assignment == null) return NotFound();
			if (assignment.Assignment.TeacherId != _userService.UserId) return Forbid();
			
			Conversation conversation = await _conversationService.GetOrCreateAsync(id, assignment.StudentId, ConversationType.StudentAssignment);
			if (conversation.Users.All(x => x.UserId != _userService.UserId)) return Forbid();
			
			return Ok(_mapper.Map<ConversationDto>(conversation));
		}
	}
}