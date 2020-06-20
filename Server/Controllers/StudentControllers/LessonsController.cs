using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EntityFrameworkCore;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.Abstractions;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Services.TeacherServices;
using LabApp.Shared.Dto;
using LabApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Controllers.StudentControllers
{
    public class LessonsController : BaseStudentController
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly AttachmentService _attachmentService;
        private readonly LessonService _lessonService;
        private readonly AssignmentService _assignmentService;
        private readonly GroupService _groupService;

        public LessonsController(AppDbContext db, IMapper mapper, IUserService userService,
            AttachmentService attachmentService, LessonService lessonService, AssignmentService assignmentService, GroupService groupService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
            _attachmentService = attachmentService;
            _lessonService = lessonService;
            _assignmentService = assignmentService;
            _groupService = groupService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LessonDto>), 200)]
        public async Task<IActionResult> Get(int groupId)
        {
            Group group = await _groupService.GetByIdAsync(groupId);
            if (group == null) return NotFound();
            if (!_groupService.StudentExists(_userService.UserId, groupId)) return Forbid();
            List<Lesson> lessons = await _db.Lessons.Where(x => x.SubjectId == group.SubjectId)
                .ToListAsync();

            return Ok(_mapper.Map<List<LessonDto>>(lessons));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LessonDto), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            Lesson lesson = await _lessonService.GetByIdAsync(id, x => x.Attachments, x => x.Subject);
            if (lesson == null) return NotFound();
            if (!_groupService.StudentExists(lesson.Subject)) return Forbid();
            LessonDto result = _mapper.Map<LessonDto>(lesson);
            result.Attachments = _mapper.Map<List<AttachmentDto>>(lesson.Attachments);

            return Ok(result);
        }
    }
}