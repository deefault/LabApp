using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Controllers.Teacher;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.Abstractions;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Server.Data.QueryModels;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Services.TeacherServices;
using LabApp.Shared.Dto;
using LabApp.Shared.Dto.Teacher;
using LabApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static LabApp.Server.Extensions.MappingExtensions;


namespace LabApp.Server.Controllers.TeacherControllers
{
	public class AssignmentsController : BaseTeacherController
	{
		private readonly AppDbContext _db;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly AssignmentService _assignmentService;
		private readonly AttachmentService _attachmentService;
		private readonly GroupService _groupService;

		public AssignmentsController(AppDbContext db, IMapper mapper, IUserService userService,
			AssignmentService assignmentService, AttachmentService attachmentService, GroupService groupService)
		{
			_db = db;
			_mapper = mapper;
			_userService = userService;
			_assignmentService = assignmentService;
			_attachmentService = attachmentService;
			_groupService = groupService;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="groupId">Включить информацию GroupAssignment (Deadline, Start, IsHidden)</param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(AssignmentDetailsDto), 200)]
		public async Task<IActionResult> GetById(int id, int? groupId = null)
		{
			Assignment assignment = await _assignmentService.GetByIdAsync(id, x => x.Subject, x => x.Attachments);
			if (assignment == null) return NotFound();
			if (assignment.Subject.TeacherId != _userService.UserId) return Forbid();

			var result = _mapper.Map<AssignmentDetailsDto>(assignment);
			if (groupId != null)
			{
				GroupAssignment ga = _assignmentService.GetGroupAssignment(groupId.Value, id);
				if (ga != null) _mapper.Map(ga, assignment);
			}
			return Ok(result);
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<AssignmentDto>), 200)]
		public async Task<IActionResult> Get(int? subjectId = null)
		{
			IEnumerable<Assignment> assignments = await _assignmentService.ListAsync(subjectId, x => x.Subject);

			return Ok(_mapper.Map<IEnumerable<AssignmentDto>>(assignments));
		}

		[HttpPost]
		[ProducesResponseType(typeof(AssignmentDetailsDto), 201)]
		public async Task<IActionResult> Add(AssignmentDetailsDto assignmentDto)
		{
			Subject subject = await _db.Subjects.FindAsync(assignmentDto.SubjectId);
			if (subject.TeacherId != _userService.UserId) return Forbid();
			Assignment assignment = await _assignmentService.AddAsync(assignmentDto, subject);
			await _db.SaveChangesAsync();

			return CreatedAtAction(nameof(GetById), new {assignment.Id}, _mapper.Map<AssignmentDetailsDto>(assignment));
		}


		[HttpPatch("{id}")]
		public async Task<IActionResult> Update(int id, AssignmentDto assignmentDto)
		{
			Assignment assignment = await _assignmentService.GetByIdAsync(id, x => x.Subject);
			if (assignment == null) return NotFound();
			if (assignment.TeacherId != _userService.UserId) return Forbid();
			_mapper.Map(assignmentDto, assignment);
			_db.Update(assignment);
			await _db.SaveChangesAsync();

			return Ok();
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			Assignment assignment = await _assignmentService.GetByIdAsync(id, x => x.Subject);
			if (assignment == null) return NotFound();
			if (assignment.TeacherId != _userService.UserId) return Forbid();
			_db.RemoveRange(assignment.Attachments);
			_db.Assignments.Remove(assignment);
			await _db.SaveChangesAsync();

			return NoContent();
		}

		#region Attachments

		/// <summary>
		/// Add attachment to existing assignment
		/// </summary>
		[HttpPut("{assignmentId}/attachments")]
		[ProducesResponseType(typeof(AttachmentDto), 200)]
		public async Task<IActionResult> AddAttachment(IFormFile file, int assignmentId)
		{
			Assignment assignment = await _assignmentService.GetByIdAsync(assignmentId);
			if (assignment == null) return NotFound();
			if (assignment.TeacherId != _userService.UserId) return Forbid();
			
			AssignmentAttachment attachment =
				await _attachmentService.AddAsync(file, AttachmentType.Assignment) as AssignmentAttachment;
			attachment.AssignmentId = assignmentId;

			await _db.SaveChangesAsync();

			return Ok(_mapper.Map<AttachmentDto>(attachment));
		}

		/// <summary>
		/// Upload attachment for new assignment
		/// </summary>
		[HttpPut("attachments")]
		[ProducesResponseType(typeof(AttachmentDto), 200)]
		public async Task<IActionResult> UploadAttachment(IFormFile file)
		{
			Attachment attachment = await _attachmentService.AddAsync(file, AttachmentType.Assignment);
			await _db.SaveChangesAsync();

			return Ok(_mapper.Map<AttachmentDto>(attachment));
		}

		[HttpDelete("attachments/{id}")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> DeleteAttachment(int id)
		{
			AssignmentAttachment entity = await _db.AssignmentAttachments.SingleOrDefaultAsync(x => x.Id == id);
			if (entity == null) return NotFound();
			if (entity.UserId != _userService.UserId) return Forbid();
			_db.AssignmentAttachments.Remove(entity);
			await _db.SaveChangesAsync();

			return Ok();
		}

		#endregion

		#region GroupAssignment

		// Move to GroupController 
		[HttpGet("group/{groupId}")]
		[ProducesResponseType(typeof(IEnumerable<AssignmentDto>), 200)]
		public async Task<IActionResult> AssignmentsByGroup(int groupId)
		{
			Group group = await _groupService.GetByIdAsync(groupId);
			if (group == null) return NotFound();
			if (group.TeacherId != _userService.UserId) return Forbid();

			List<AssignmentWithGroupInfo> assignments = await _assignmentService.ListWithGroupInfoAsync(group, x => x.Subject);

			return Ok(_mapper.Map<IEnumerable<AssignmentDto>>(assignments));
		}

		#endregion
	}
}