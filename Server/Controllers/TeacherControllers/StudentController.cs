using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers.Teacher
{
    // override
    [Route("api/[controller]/")]
    public class StudentController : BaseStudentController
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        
        public StudentController(IUserService userService, AppDbContext db, IMapper mapper, IUserRepository userRepository)
        {
            _userService = userService;
            _db = db;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Data.Models.Student user = _userRepository.GetById(id) as Data.Models.Student;
            
            return Ok(_mapper.Map<StudentProfileDto>(user));
        }
    }
}