using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Data.Repositories.Abstractions;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace LabApp.Server.Services
{
    public class LessonService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public LessonService(AppDbContext db, IMapper mapper, IUserService userService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Lesson> GetByIdAsync(int id, params Expression<Func<Lesson, object>>[] includes)
        {
            return await _db.Set<Lesson>().AddIncludes(includes).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Lesson> AddAsync(LessonDto lessonDto)
        {
            Lesson lesson = (await _db.Lessons.AddAsync(_mapper.Map<LessonDto, Lesson>(lessonDto))).Entity;
            await _db.SaveChangesAsync();
            await _db.LessonAttachments.Where(x => lessonDto.Attachments.Select(y => y.Id).Contains(x.Id) && 
                                                   x.UserId == _userService.UserId)
                .UpdateAsync(x => new LessonAttachment {LessonId = lesson.Id});
            await _db.Entry(lesson).Collection(x => x.Attachments).LoadAsync();

            return lesson;
        }
    }
}