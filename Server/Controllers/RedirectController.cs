using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers
{
    [Route("/")]
    public class RedirectController : Controller
    {
        private readonly GroupService _groupService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public RedirectController(GroupService groupService, IUserService userService, AppDbContext db, IMapper mapper)
        {
            _groupService = groupService;
            _userService = userService;
            _db = db;
            _mapper = mapper;
        }

        [Route("JoinGroup")]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> JoinGroup([FromQuery] string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return BadRequest();
            string url = System.Uri.EscapeUriString(HttpContext.Request.Path + HttpContext.Request.QueryString); 
            if (!HttpContext.User?.Identity?.IsAuthenticated ?? false)
                return Redirect($"/login?redirectTo={url})");
            var student = _userService.CurrentUser as Data.Models.Student;
            if (student == null) return Forbid();
            
            Group group = await _groupService.GetByCodeAsync(code);
            if (group == null) return NotFound();
            _groupService.AddStudent(student, group);
            await _db.SaveChangesAsync();
            
            return RedirectToAction("Get", "Groups", new {id = group.Id});
        }

    }
}