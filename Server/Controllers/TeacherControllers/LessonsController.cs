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

namespace LabApp.Server.Controllers.TeacherControllers
{
    public class LessonsController : BaseTeacherController
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly AttachmentService _attachmentService;
        private readonly LessonService _lessonService;
        private readonly AssignmentService _assignmentService;

        public LessonsController(AppDbContext db, IMapper mapper, IUserService userService,
            AttachmentService attachmentService, LessonService lessonService, AssignmentService assignmentService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
            _attachmentService = attachmentService;
            _lessonService = lessonService;
            _assignmentService = assignmentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LessonDto>), 200)]
        public async Task<IActionResult> Get(int? subjectId)
        {
            List<Lesson> lessons = await _db.Lessons.Where(x => subjectId == null || x.SubjectId == subjectId &&
                    x.Subject.TeacherId == _userService.UserId)
                .ToListAsync();

            return Ok(_mapper.Map<List<LessonDto>>(lessons));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LessonDto), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            Lesson lesson = await _lessonService.GetByIdAsync(id, x => x.Attachments, x => x.Subject);
            if (lesson == null) return NotFound();
            if (lesson.Subject.TeacherId != _userService.UserId) return Forbid();
            LessonDto result = _mapper.Map<LessonDto>(lesson);
            result.Attachments = _mapper.Map<List<AttachmentDto>>(lesson.Attachments);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LessonDto), 201)]
        public async Task<IActionResult> Add(LessonDto lessonDto)
        {
            Subject subject = await _db.Subjects.FindAsync(lessonDto.SubjectId);
            if (subject.TeacherId != _userService.UserId) return Forbid();
            Lesson lesson = await _lessonService.AddAsync(lessonDto);

            return CreatedAtAction(nameof(GetById), new {id = lesson.Id}, _mapper.Map<LessonDto>(lesson));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, LessonDto lessonDto)
        {
            Lesson lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();
            if (lesson.Subject.TeacherId != _userService.UserId) return Forbid();
            _mapper.Map(lessonDto, lesson);
            _db.Update(lesson);
            await _db.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Lesson lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();
            if (lesson.Subject.TeacherId != _userService.UserId) return Forbid();
            _db.RemoveRange(lesson.Attachments);
            _db.Lessons.Remove(lesson);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Attach assignment to lesson
        /// </summary>
        /// <remarks> Replaces if other assignment is attached</remarks>
        [HttpPut("{id}/assignment")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AttachAssignment([FromRoute] int id, [FromBody] int assignmentId)
        {
            Lesson lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();
            if (lesson.Subject.TeacherId != _userService.UserId) return Forbid();
            Assignment assignment = await _assignmentService.GetByIdAsync(assignmentId, x => x.Subject);
            if (assignment == null) return NotFound();
            if (assignment.Subject.TeacherId != _userService.UserId) return Forbid();

            lesson.Assignment = assignment;
            _db.Update(lesson);
            await _db.SaveChangesAsync();

            return Ok();
        }
        
        /// <summary>
        /// Remove assignment from lesson
        /// </summary>
        [HttpDelete("{id}/assignment")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RemoveAssignment([FromRoute] int id)
        {
            Lesson lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();
            if (lesson.Subject.TeacherId != _userService.UserId) return Forbid();

            lesson.Assignment = null;
            _db.Update(lesson);
            await _db.SaveChangesAsync();

            return Ok();
        }

        #region Attachments

        [HttpPut("{lessonId}/attachments")]
        [ProducesResponseType(typeof(AttachmentDto), 200)]
        public async Task<IActionResult> AddAttachment(IFormFile file, int lessonId)
        {
            LessonAttachment attachment =
                await _attachmentService.AddAsync(file, AttachmentType.Lesson) as LessonAttachment;
            attachment.LessonId = lessonId;

            await _db.SaveChangesAsync();

            return Ok(_mapper.Map<AttachmentDto>(attachment));
        }

        [HttpPut("attachments")]
        [ProducesResponseType(typeof(AttachmentDto), 200)]
        public async Task<IActionResult> UploadAttachment(IFormFile file)
        {
            Attachment attachment = await _attachmentService.AddAsync(file, AttachmentType.Lesson);
            await _db.SaveChangesAsync();

            return Ok(_mapper.Map<AttachmentDto>(attachment));
        }

        [HttpDelete("attachments/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            LessonAttachment entity = await _db.LessonAttachments.SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();
            _db.LessonAttachments.Remove(entity);
            await _db.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}