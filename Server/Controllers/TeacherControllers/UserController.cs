using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Teacher = LabApp.Server.Data.Models.Teacher;

namespace LabApp.Server.Controllers.Teacher
{
    public class UserController : BaseTeacherController
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
        [ProducesResponseType(typeof(TeacherProfileDto), 200)]
        public async Task<IActionResult> Get()
        {
            int id = _userService.UserId;
            Data.Models.Teacher user = _userRepository.GetById(id) as Data.Models.Teacher;
            await _db.Entry(user!).Reference(x => x.AcademicRank).LoadAsync();
            
            return Ok(_mapper.Map<TeacherProfileDto>(user));
        }
        
        [HttpPatch]
        public IActionResult Edit(TeacherProfileDto profile)
        {
            int id = _userService.UserId;
            Data.Models.Teacher user = _userRepository.GetById(id) as Data.Models.Teacher;
            
            _mapper.Map(profile, user);
            _db.SaveChanges();
            
            return Ok();
        }
    }
}