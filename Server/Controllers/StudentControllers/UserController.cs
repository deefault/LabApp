using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers.Student
{
    public class UserController : BaseStudentController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IUserService userService, AppDbContext db, IMapper mapper)
        {
            _userRepository = userRepository;
            _userService = userService;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(StudentProfileDto), 200)]
        public IActionResult Get()
        {
            int id = _userService.UserId;
            Data.Models.Student user = _userRepository.GetById(id) as Data.Models.Student;

            return Ok(_mapper.Map<StudentProfileDto>(user));
        }
        
        
        [HttpPatch]
        public IActionResult Edit(StudentProfileDto profile)
        {
            int id = _userService.UserId;
            Data.Models.Student user = _userRepository.GetById(id) as Data.Models.Student;
            
            _mapper.Map(profile, user);
            _db.SaveChanges();
            
            return Ok();
        }
    }
}