using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using Microsoft.AspNetCore.Http;

namespace LabApp.Server.Services
{
	public class HistoryService
	{
		private readonly AppDbContext _db;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HistoryService(AppDbContext db, IHttpContextAccessor httpContextAccessor)
		{
			_db = db;
			_httpContextAccessor = httpContextAccessor;
		}


		public void UserLogged(User user)
		{
			var context = _httpContextAccessor.HttpContext;
			_db.LoginHistory.Add(new UserLoginHistory()
			{
				Address = context.Connection.RemoteIpAddress,
				UserId = user.UserId,
				UserAgent = context.Request.Headers["User-Agent"]
			});
			_db.SaveChanges();
		}
	}
}