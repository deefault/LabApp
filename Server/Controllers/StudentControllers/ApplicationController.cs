using System.Threading.Tasks;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Services;
using LabApp.Shared.Dto.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace LabApp.Server.Controllers.StudentControllers
{
    public class ApplicationController : BaseStudentController
    {
        private readonly IHostEnvironment _environment;
        private readonly ConversationService _conversationService;

        public ApplicationController(IHostEnvironment environment,
            ConversationService conversationService)
        {
            _environment = environment;
            _conversationService = conversationService;
        }
        
        [HttpGet("init")]
        [ProducesResponseType(typeof(InitDtoStudent), 200)]
        public async Task<InitDtoStudent> Init()
        {
            return new InitDtoStudent{NewMessages = await _conversationService.CountNewAsync()};
        } 
    }
}