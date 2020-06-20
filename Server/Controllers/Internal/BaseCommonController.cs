using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers.Internal
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "common")]
    public abstract class BaseCommonController : BaseController
    {
    }
}