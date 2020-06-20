using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Services.TeacherServices;
using LabApp.Shared.Dto;
using LabApp.Shared.Dto.Teacher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Controllers.TeacherControllers
{
	public class GroupController : BaseTeacherController
	{
		private readonly IUserService _userService;
		private readonly IUserRepository _userRepository;
		private readonly AppDbContext _db;
		private readonly IMapper _mapper;
		private readonly GroupService _groupService;
		private readonly AssignmentService _assignmentService;

		public GroupController(IUserService userService, AppDbContext db, IMapper mapper,
			IUserRepository userRepository, GroupService groupService, AssignmentService assignmentService)
		{
			_userService = userService;
			_db = db;
			_mapper = mapper;
			_userRepository = userRepository;
			_groupService = groupService;
			_assignmentService = assignmentService;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GroupDetailsTeacherDto), 200)]
		public async Task<IActionResult> Get(int id)
		{
			Group group = await _groupService.GetByIdAsync(id);
			if (group == null) return NotFound();
			if (group.TeacherId != _userService.UserId) return Forbid();

			return Ok(_mapper.Map<GroupDetailsTeacherDto>(group));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<GroupDto>), 200)]
		public IActionResult List(int page = 1, int pageSize = 50)
		{
			Data.Models.Teacher user = _userService.CurrentUser as Data.Models.Teacher;

			IEnumerable<Group> groups = _groupService.Get(user);

			return Ok(_mapper.Map<IEnumerable<Group>, IEnumerable<GroupDto>>(groups));
		}

		[HttpPost]
		public IActionResult Add(GroupDto groupDto)
		{
			groupDto.TeacherId = _userService.UserId;
			Group group = _groupService.Add(_mapper.Map<Group>(groupDto));
			_db.SaveChanges();

			return CreatedAtAction(nameof(Add), new {id = group.Id}, group);
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] GroupDto groupDto)
		{
			Group group = await _groupService.GetByIdAsync(id);
			if (group == null) return NotFound();
			if (group.TeacherId != _userService.UserId) return Forbid();
			_mapper.Map(groupDto, group, opt => opt.BeforeMap((src, dest) =>
				{
					src.SubjectId = dest.SubjectId;
					src.TeacherId = dest.TeacherId;
				})
			);
			await _db.SaveChangesAsync();

			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			Group group = await _groupService.GetByIdAsync(id);
			if (group == null) return NotFound();
			if (group.TeacherId != _userService.UserId) return Forbid();

			_db.Groups.Remove(group);
			await _db.SaveChangesAsync();

			return Ok();
		}

		/// <summary>
		/// Add student to group
		/// </summary>
		[HttpPut("{groupId}")]
		[ProducesResponseType(typeof(UserListDto), 200)]
		public async Task<IActionResult> AddStudent(int groupId, string email = null, int? id = null)
		{
			if (string.IsNullOrWhiteSpace(email) && id == null) return BadRequest("No groupId or email specified");
			if (!new EmailAddressAttribute().IsValid(email))
			{
				ModelState.AddModelError("email", "Неверный email");
				return BadRequest(ModelState);
			}

			Group group = await _groupService.GetByIdAsync(groupId);
			if (group == null) return NotFound("No group");
			if (group.TeacherId != _userService.UserId) return Forbid();

			Data.Models.Student student = _userRepository.FindUserByEmail(email) as Data.Models.Student;
			if (student == null) return NotFound();
			if (_groupService.StudentExists(student.UserId, groupId)) return BadRequest("Пользователь уже добавлен");
			_groupService.AddStudent(student, group);
			await _db.SaveChangesAsync();

			return Ok(_mapper.Map<UserListDto>(student));
		}

		[HttpGet("{groupId}/students")]
		[ProducesResponseType(typeof(IEnumerable<UserListDto>), 200)]
		public async Task<IActionResult> Students(int groupId)
		{
			Group group = await _groupService.GetByIdAsync(groupId);
			if (group == null) return NotFound();
			if (group.TeacherId != _userService.UserId) return Forbid();

			var students = await _db.Entry(group).Collection(x => x.Students).Query().Select(x => x.Student)
				.ToListAsync();

			return Ok(_mapper.Map<IEnumerable<UserListDto>>(students));
		}
		
		public class GroupTableData
		{
			public Data.Models.Student Student { get; set; }

			public IEnumerable<StudentAssignment> StudentAssignments { get; set; } 
		}

		[HttpGet("{groupId}/table")]
		[ProducesResponseType(typeof(GroupTableDto), 200)]
		public async Task<IActionResult> StudentScoreTable(int groupId)
		{
			Group group = await _groupService.GetByIdAsync(groupId);
			if (group == null) return NotFound();
			if (group.TeacherId != _userService.UserId) return Forbid();

			var table = await _db.Entry(group).Collection(x => x.Students).Query()
				.Select(x => new GroupTableData
				{
					Student = x.Student,
					StudentAssignments =
						_db.StudentAssignment.Where(y => y.StudentId == x.StudentId && y.GroupId == groupId)
				}).ToListAsync();

			List<AdditionalScore> additionalScores = await _db.AdditionalScores.Where(x => x.GroupId == groupId).ToListAsync();
			IEnumerable<Assignment> assignments = await _assignmentService.ListByGroupAsync(group);

			return Ok(new GroupTableDto
			{
				Assignments = _mapper.Map<IEnumerable<AssignmentDto>>(assignments),
				Entries = _mapper.Map<IEnumerable<GroupTableEntryDto>>(table)
			});
		}
	}
}