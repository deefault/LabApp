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
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers.StudentControllers
{
    public class GroupsController : BaseStudentController
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly GroupService _groupService;

        public GroupsController(IUserService userService, AppDbContext db, IMapper mapper,
            IUserRepository userRepository, GroupService groupService)
        {
            _userService = userService;
            _db = db;
            _mapper = mapper;
            _userRepository = userRepository;
            _groupService = groupService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupDto), 200)]
        public async Task<IActionResult> Get(int id)
        {
            Group group = await _groupService.GetByIdAsync(id);
            if (group == null) return NotFound();
            if (!_groupService.StudentExists(_userService.UserId, group.Id)) return Forbid();

            return Ok(_mapper.Map<GroupDto>(group));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GroupDto>), 200)]
        public IActionResult List()
        {
            var student = _userService.CurrentUser as Data.Models.Student;

            return Ok(_mapper.Map<IEnumerable<GroupDto>>(student.Groups.Select(x => x.Group).ToList()));
        }


        /// <summary>
        /// Add student to group
        /// </summary>
        [HttpPost("{groupId}/join")]
        [ProducesResponseType(typeof(UserListDto), 200)]
        public async Task<IActionResult> Join(int groupId)
        {
            Group group = await _groupService.GetByIdAsync(groupId);
            if (group == null) return NotFound("No group");
            
            var student = _userService.CurrentUser as Data.Models.Student;
            if (student == null) return NotFound();
            if (_groupService.StudentExists(student.UserId, groupId)) return BadRequest("Пользователь уже добавлен");
            _groupService.AddStudent(student, group);
            await _db.SaveChangesAsync();

            return Ok(_mapper.Map<UserListDto>(student));
        }
        
        /// <summary>
        /// Add student to group by code
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserListDto), 200)]
        public async Task<IActionResult> JoinByCode([FromQuery] string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return BadRequest(); 
            Group group = await _groupService.GetByCodeAsync(code);
            if (group == null) return NotFound("No group");
            
            var student = _userService.CurrentUser as Data.Models.Student;
            if (student == null) return NotFound();
            if (_groupService.StudentExists(student.UserId, group.Id)) return BadRequest("Пользователь уже добавлен");
            _groupService.AddStudent(student, group);
            await _db.SaveChangesAsync();

            return Ok(_mapper.Map<UserListDto>(student));
        }

        /*[HttpGet("{groupId}/students")]
        [ProducesResponseType(typeof(IEnumerable<UserListDto>), 200)]
        public async Task<IActionResult> Students(int groupId)
        {
            Group group = await _groupService.GetByIdAsync(groupId);
            if (group == null) return NotFound();
            if (group.TeacherId != _userService.CurrentUserId) return Forbid();

            var students = await _db.Entry(group).Collection(x => x.Students).Query().Select(x => x.Student)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<UserListDto>>(students));
        }*/
    }
}