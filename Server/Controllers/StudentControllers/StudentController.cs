using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data.MapperProfiles;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LabApp.Shared.Enums;

namespace LabApp.Server.Controllers.Student
{
    // override
    [Route("api/[area]/[action]")]
    public class StudentController : BaseStudentController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public StudentController(IUserRepository userRepository, IUserService userService, IMapper mapper)
        {
            _userRepository = userRepository;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody] StudentRegisterDto data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (_userRepository.EmailExists(data.Email))
            {
                ModelState.AddModelError("", "Email exists");
                return BadRequest("Данный email уже зарегистрирован");
            }
            User user = _userService.RegisterUser(data, UserType.Student);

            return Ok(_mapper.Map<User, ProfileDto>(user));
        }
    }
}