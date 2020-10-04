using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.Abstractions;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Hubs;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Services.StudentServices;
using LabApp.Shared.Dto;
using LabApp.Shared.Dto.Student;
using LabApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LabApp.Server.Controllers.StudentControllers
{
	public class AssignmentsController : BaseStudentController
	{
		private readonly AppDbContext _db;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly GroupService _groupService;
		private readonly AssignmentService _assignmentService;
		private readonly AttachmentService _attachmentService;
		private readonly ConversationService _conversationService;

		public AssignmentsController(AppDbContext db, IMapper mapper, IUserService userService,
			AssignmentService assignmentService, AttachmentService attachmentService, GroupService groupService, ConversationService conversationService)
		{
			_db = db;
			_mapper = mapper;
			_userService = userService;
			_assignmentService = assignmentService;
			_attachmentService = attachmentService;
			_groupService = groupService;
			_conversationService = conversationService;
		}

		/// <summary>
		/// Get assignment by id
		/// </summary>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(AssignmentDetailsDtoStudent), 200)]
		public async Task<IActionResult> GetById(int id)
		{
			AssignmentWithConcrete assignment =
				await _assignmentService.GetAssignmentById(id, _userService.UserId, x => x.Subject,
					x => x.Attachments);
			if (assignment == null) return NotFound();
			if (!await _assignmentService.CheckUserAsync(assignment.Assignment, _userService.UserId)) return Forbid();

			return Ok(_mapper.Map<AssignmentDetailsDtoStudent>(assignment));
		}

		/// <summary>
		/// Gets concrete student assignment by StudentAssignment Id
		/// </summary>
		/// <param name="id">StudentAssignment Id</param>
		[HttpGet("student-assignments/{id}")]
		[ProducesResponseType(typeof(StudentAssignmentDto), 200)]
		public async Task<IActionResult> GetStudentAssignmentById(int id)
		{
			StudentAssignment assignment = await _assignmentService.GetStudentAssignmentById(id);
			if (assignment == null) return NotFound();
			if (assignment.StudentId != _userService.UserId) return Forbid();

			return Ok(_mapper.Map<StudentAssignmentDto>(assignment));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<AssignmentDtoStudent>), 200)]
		public async Task<IActionResult> Get(int? groupId = null)
		{
			Group group = null;
			if (groupId.HasValue)
			{
				group = await _groupService.GetByIdAsync(groupId.Value);
				if (group == null) return NotFound();
			}

			IEnumerable<AssignmentWithConcrete> assignments = groupId.HasValue
				? await _assignmentService.GetStudentAssignmentsAsync(_userService.UserId, group)
				: await _assignmentService.GetStudentAssignmentsAsync(_userService.UserId);

			return Ok(_mapper.Map<IEnumerable<AssignmentDtoStudent>>(assignments));
		}

		[HttpPut("{assignmentId}")]
		[ProducesResponseType(typeof(StudentAssignmentDto), 201)]
		public async Task<IActionResult> Submit(int assignmentId, StudentAssignmentDto assignmentDto)
		{
			#region Checks

			AssignmentWithConcrete assignment =
				await _assignmentService.GetAssignmentById(assignmentId, _userService.UserId);
			if (assignment == null) return NotFound();
			if (assignment.StudentAssignment != null) return Conflict();

			Group group = await _groupService.GetByIdAsync(assignmentDto.GroupId);
			if (group == null) return NotFound();
			if (!_groupService.StudentExists(_userService.UserId, group.Id)) return Forbid();

			#endregion

			StudentAssignment studentAssignment =
				_assignmentService.Add(assignmentDto, _userService.CurrentUser, assignmentId);
			studentAssignment.GroupId = group.Id;
			await _db.SaveChangesAsync();

			//await _teacherHub.NewAssignment(assignmentId, studentAssignment.Id, assignment.Assignment.TeacherId);

			return CreatedAtAction(nameof(GetById), new {studentAssignment.Id},
				_mapper.Map<StudentAssignmentDto>(assignment.StudentAssignment));
		}


		[HttpPatch("{id}")]
		public async Task<IActionResult> Update(int id, StudentAssignmentDto assignmentDto)
		{
			StudentAssignment assignment = await _assignmentService.GetStudentAssignmentById(id);
			if (assignment == null) return NotFound();
			if (assignment.StudentId != _userService.UserId) return Forbid();
			if (!_assignmentService.IsAllowedToUpdate(assignment))
				return BadRequest("Вы не можете редактировать работу");

			assignmentDto.AssignmentId = assignment.Id;
			_mapper.Map(assignmentDto, assignment);
			_db.Update(assignment);

			await _db.SaveChangesAsync();

			return Ok();
		}

		[HttpPatch("{id}/needReview")]
		public async Task<IActionResult> NeedReview(int id)
		{
			StudentAssignment assignment = await _assignmentService.GetStudentAssignmentById(id);
			if (assignment == null) return NotFound();
			if (assignment.StudentId != _userService.UserId) return Forbid();
			if (assignment.Status != AssignmentStatus.ChangesRequested)
				return BadRequest("Вы не можете изменить статус");

			_assignmentService.NeedReview(assignment);
			await _db.SaveChangesAsync();

			// TODO:
			//await _teacherHub.NewAssignment(assignment.AssignmentId, id, assignment.Assignment.TeacherId);
			
			return Ok();
		}

		#region Attachments

		/// <summary>
		/// Upload attachment for new or existing assignment
		/// </summary>
		[HttpPut("attachments")]
		[ProducesResponseType(typeof(AttachmentDto), 200)]
		public async Task<IActionResult> UploadAttachment(IFormFile file, int? assignmentId)
		{
			StudentAssignmentAttachment attachment =
				await _attachmentService.AddAsync(file,
					AttachmentType.StudentAssignment) as StudentAssignmentAttachment;
			if (assignmentId.HasValue)
			{
				StudentAssignment assignment = await _assignmentService.GetStudentAssignmentById(assignmentId.Value);
				if (assignment == null) return NotFound();
				if (assignment.StudentId != _userService.UserId) return Forbid();
				attachment.Assignment = assignment;
			}
			await _db.SaveChangesAsync();

			return Ok(_mapper.Map<AttachmentDto>(attachment));
		}

		[HttpDelete("attachments/{id}")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> DeleteAttachment(int id)
		{
			StudentAssignmentAttachment entity = await _db.StudentAssignmentAttachment.SingleOrDefaultAsync(x => x.Id == id);
			if (entity == null) return NotFound();
			if (entity.UserId != _userService.UserId) return Forbid();
			_db.StudentAssignmentAttachment.Remove(entity);
			await _db.SaveChangesAsync();

			return Ok();
		}

		#endregion
		
		[HttpGet("{id}/conversation/")]
		[ProducesResponseType(typeof(ConversationDto), 200)]
		public async Task<IActionResult> GetConversation(int id)
		{
			StudentAssignment assignment = await _assignmentService.GetStudentAssignmentById(id);
			if (assignment == null) return NotFound();
			if (assignment.StudentId != _userService.UserId) return Forbid();

			Conversation conversation = await _conversationService.GetOrCreateAsync(id, assignment.Assignment.TeacherId, ConversationType.StudentAssignment);

			return Ok(_mapper.Map<ConversationDto>(conversation));
		}
	}
}