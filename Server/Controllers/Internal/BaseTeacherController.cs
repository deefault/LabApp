using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers.Internal
{
    [Area("Teacher")]
    [Route("api/[area]/[controller]")]
    [Authorize(Roles = "Teacher")]
    [ApiExplorerSettings(GroupName = "teacher")]  
    public abstract class BaseTeacherController : BaseController
    {
        
    }
}