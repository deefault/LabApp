using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data.MapperProfiles;
using LabApp.Server.Data.Models;
using LabApp.Shared.Enums;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers.Teacher
{
    // override
    [Route("api/[area]/[action]")]
    public class TeacherController : BaseTeacherController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public TeacherController(IUserRepository userRepository, IUserService userService, IMapper mapper)
        {
            _userRepository = userRepository;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody]TeacherRegisterDto data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (_userRepository.EmailExists(data.Email))
            {
                ModelState.AddModelError("", "Email exists");
                return BadRequest("Данный email уже зарегистрирован");
            }
            User user = _userService.RegisterUser(data, UserType.Teacher);
            
            return Ok(_mapper.Map<User, ProfileDto>(user));
        }
    }
}