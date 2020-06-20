using System;
using System.IO;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers
{
    public class ResourceController : BaseCommonController
    {
        // GET

        private readonly AppDbContext _db;
        private readonly IFileStorage _fileStorage;

        public ResourceController(AppDbContext db, IFileStorage fileStorage)
        {
            _db = db;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [Route("/Download/Attachment/{name}", Name = "Attachment")]
        public IActionResult Attachment(string name, string downloadName)
        {
            var path = _fileStorage.GetFile(name);
            if (path == null) return NotFound();

            try
            {
                return PhysicalFile(path, "application/octet-stream", downloadName);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            
        }
        
        
        [HttpGet]
        [Route("/Download/Image/{name}", Name = "Image")]
        public IActionResult Image(string name)
        {
            var path = _fileStorage.GetImage(name);
            if (path == null) return NotFound();
            
            return PhysicalFile(path, "image/jpeg");
        }
        
        [HttpGet]
        [Route("/Download/Thumbnail/{image}", Name = "Thumbnail")]
        public IActionResult Thumbnail(string name)
        {
            var path = _fileStorage.GetImageThumbnail(name);
            if (path == null) return NotFound();
            
            return PhysicalFile(path, "image/jpeg");
        }
    }
}