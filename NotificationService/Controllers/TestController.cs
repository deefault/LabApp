using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly IServiceProvider _sp;

        public TestController(IServiceProvider sp)
        {
            _sp = sp;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            //await _sp.GetRequiredService<IRealtimeNotificationService>().NewMessage(new NewMessageDto() {Users = new []{1}});
            // await _sp.GetRequiredService<IHubContext<CommonHub>>().Clients.All.SendAsync("NewMessage", new NewMessageDto());
            // await _sp.GetRequiredService<IHubContext<CommonHub>>().Clients.User("1")
            //     .SendAsync("NewMessage", new NewMessageDto());
            // await _sp.GetRequiredService<IHubContext<CommonHub>>().Clients.User("1")
            //     .SendAsync("newMessage", new NewMessageDto());

            return Ok();
        }
    }
}