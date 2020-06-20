using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Services.StudentServices;
using LabApp.Shared.Dto;
using LabApp.Shared.Dto.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Controllers.StudentControllers
{
    public class SubjectsController : BaseStudentController
    {
        private readonly IUserService _userService;
        private readonly GroupService _groupService;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly AssignmentService _assignmentService;

        public SubjectsController(IUserService userService, AppDbContext db, IMapper mapper, GroupService groupService,
            AssignmentService assignmentService)
        {
            _userService = userService;
            _db = db;
            _mapper = mapper;
            _groupService = groupService;
            _assignmentService = assignmentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GroupSubjectDto>), 200)]
        public async Task<IActionResult> Get([FromQuery] Paging paging = default)
        {
            Data.Models.Student student = _userService.CurrentUser as Data.Models.Student;
            IEnumerable<Group> group = await _groupService.GetGroupsAsync(student);

            return Ok(_mapper.Map<IEnumerable<Group>, IEnumerable<GroupSubjectDto>>(group));
        }

        /// <summary>
        /// Get subject by groupId (subject can have many groups)
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet("{groupId}")]
        [ProducesResponseType(typeof(GroupSubjectDto), 200)]
        public async Task<IActionResult> GetByGroupId(int groupId)
        {
            Group group = await _groupService.GetByIdAsync(groupId, x => x.Teacher, x => x.Subject);
            if (group == null) return NotFound();
            if (!_groupService.StudentExists(_userService.UserId, groupId)) return Forbid();

            return Ok(_mapper.Map<GroupSubjectDto>(group));
        }

        [HttpGet("{groupId}/assignments/")]
        [ProducesResponseType(typeof(IEnumerable<AssignmentDtoStudent>), 200)]
        public async Task<IActionResult> Assignments(int groupId)
        {
            Group group = await _groupService.GetByIdAsync(groupId, x => x.Teacher, x => x.Subject);
            if (group == null) return NotFound();
            if (!_groupService.StudentExists(_userService.UserId, groupId)) return Forbid();

            IEnumerable<AssignmentWithConcrete> assignments = await 
                _assignmentService.GetStudentAssignmentsAsync(_userService.UserId, group);

            return Ok(_mapper.Map<AssignmentDtoStudent>(assignments));
        }
    }
}