using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers.StudentControllers
{
    [Route("api/[controller]/")] //override
    public class TeacherController : BaseStudentController
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        
        public TeacherController(IUserService userService, AppDbContext db, IMapper mapper, IUserRepository userRepository)
        {
            _userService = userService;
            _db = db;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Data.Models.Teacher user = _userRepository.GetById(id) as Data.Models.Teacher;
            await _db.Entry(user!).Reference(x => x.AcademicRank).LoadAsync();
            
            return Ok(_mapper.Map<TeacherProfileDto>(user));
        }
    }
}