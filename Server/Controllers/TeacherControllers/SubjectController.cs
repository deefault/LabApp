using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Services.StudentServices;
using LabApp.Shared.Dto;
using LabApp.Shared.Dto.Teacher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssignmentService = LabApp.Server.Services.TeacherServices.AssignmentService;

namespace LabApp.Server.Controllers.TeacherControllers
{
    public class SubjectController : BaseTeacherController
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly AssignmentService _assignmentService;

        public SubjectController(IUserService userService, AppDbContext db, IMapper mapper,
            IUserRepository userRepository, AssignmentService assignmentService)
        {
            _userService = userService;
            _db = db;
            _mapper = mapper;
            _userRepository = userRepository;
            _assignmentService = assignmentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubjectDto>), 200)]
        public IActionResult Get([FromQuery] Paging paging = default)
        {
            IEnumerable<Subject> subjects = _db.Subjects.Where(x => x.TeacherId == _userService.UserId)
                .ToPagedList(paging);

            return Ok(_mapper.Map<IEnumerable<Subject>, IEnumerable<SubjectDto>>(subjects));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SubjectDto), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            Subject subject = await GetByIdAsync(id);
            if (subject == null) return NotFound();
            if (subject.TeacherId != _userService.UserId) return Forbid();

            return Ok(_mapper.Map<SubjectDto>(subject));
        }

        [HttpPost]
        public IActionResult Add(SubjectDto subjectDto)
        {
            subjectDto.TeacherId = _userService.UserId;
            Subject subject = _db.Subjects.Add(_mapper.Map<Subject>(subjectDto)).Entity;
            _db.SaveChanges();

            return CreatedAtAction(nameof(Add), new {id = subject.Id}, subject);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SubjectDto subjectDto)
        {
            Subject subject = await GetByIdAsync(id);
            if (subject == null) return NotFound();
            if (subject.TeacherId != _userService.UserId) return Forbid();
            _mapper.Map(subjectDto, subject, opt => opt.BeforeMap((src, dest) => src.TeacherId = dest.TeacherId));
            _db.Update(subject);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Subject subject = await GetByIdAsync(id);
            if (subject == null) return NotFound();
            if (subject.TeacherId != _userService.UserId) return Forbid();

            _db.Remove(subject);
            await _db.SaveChangesAsync();

            return Ok();
        }
        
        [HttpGet("{subjectId}/assignments/")]
        [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), 200)]
        public async Task<IActionResult> Assignments(int subjectId)
        {
            IEnumerable<Assignment> assignments = await _assignmentService.ListAsync(subjectId, x => x.Subject);

            return Ok(_mapper.Map<IEnumerable<AssignmentDto>>(assignments));
        }

        private async Task<Subject> GetByIdAsync(int id)
        {
            return await _db.Subjects.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}