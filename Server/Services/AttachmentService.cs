using System;
using System.Threading.Tasks;
using LabApp.Server.Data;
using LabApp.Server.Data.Models.Abstractions;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using LabApp.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace LabApp.Server.Services
{
    public class AttachmentService
    {
        private readonly IFileStorage _fileStorage;
        private readonly AppDbContext _db;
        private readonly IUserService _userService;

        public AttachmentService(IFileStorage fileStorage, AppDbContext db, IUserService userService)
        {
            _db = db;
            _fileStorage = fileStorage;
            _userService = userService;
        }
        
        public async Task<Attachment> AddAsync(IFormFile file, AttachmentType type)
        {
            string filename = Guid.NewGuid().ToString();
            if (!await _fileStorage.UploadFileAsync(file.OpenReadStream(), filename))
                throw new Exception("File not uploaded!");
            Attachment attachment = CreateAttachmentByType(type);
            attachment.Name = file.FileName;
            attachment.Size = file.Length;
            attachment.Url = filename;
            attachment.UserId = _userService.UserId;
            var result = await _db.AddAsync(attachment);
            await _db.SaveChangesAsync(); 
            
            return result.Entity;
        }

        private Attachment CreateAttachmentByType(AttachmentType type)
        {
            return type switch
            {
                AttachmentType.Lesson => new LessonAttachment(),
                AttachmentType.Assignment => new AssignmentAttachment(),
                AttachmentType.StudentAssignment => new StudentAssignmentAttachment(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}