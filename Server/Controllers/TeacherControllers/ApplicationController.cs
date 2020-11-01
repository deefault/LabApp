using System;
using System.Threading.Tasks;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Services.TeacherServices;
using LabApp.Shared.Dto.Student;
using LabApp.Shared.Dto.Teacher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace LabApp.Server.Controllers.TeacherControllers
{
    public class ApplicationController : BaseTeacherController
    {
        private readonly IHostEnvironment _environment;
        private readonly StudentAssignmentService _studentAssignmentService;

        public ApplicationController(IHostEnvironment environment, StudentAssignmentService studentAssignmentService)
        {
            _environment = environment;
            _studentAssignmentService = studentAssignmentService;
        }

        [HttpGet]
        public string GetImageDownloadPath()
        {
            return _environment.IsDevelopment()
                ? new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value, "Download").ToString()
                : AppConfiguration.StorageUrl;
        }

        [HttpGet("init")]
        [ProducesResponseType(typeof(InitDtoTeacher), 200)]
        public async Task<InitDtoTeacher> Init()
        {
            return new InitDtoTeacher{NewAssignmentsCount = await _studentAssignmentService.CountAsync(onlyNew: true)};
        } 
    }
}