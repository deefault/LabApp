using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers.Internal
{
    [Area("Student")]
    [Route("api/[area]/[controller]/[action]")]
    [Authorize(Roles = "Student")]
    [ApiExplorerSettings(GroupName = "student")]  
    public abstract class BaseStudentController : BaseController
    {
    }
}