using System;
using System.Threading.Tasks;
using LabApp.Server.Controllers.Internal;
using LabApp.Shared.Dto.Teacher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace LabApp.Server.Controllers
{
    public class ApplicationController : BaseCommonController
    {
        private readonly IHostEnvironment _environment;

        public ApplicationController(IHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public string GetImageDownloadPath()
        {
            return _environment.IsDevelopment()
                ? new UriBuilder(AppConfiguration.Host, "/Download/Attachment").ToString()
                : AppConfiguration.StorageUrl;
        }
        
        [HttpGet]
        public object GetDownloadPaths()
        {
            return new
            {
                AttachmentPath = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value,
                    "/Download/Attachment").ToString(),
                ImagePath = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value,
                    "/Download/Image").ToString(),
                ThumbnailPath = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value,
                    "/Download/Thumbnail").ToString()
            };
        }
    }
}