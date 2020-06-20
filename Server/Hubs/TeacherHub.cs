using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
using LabApp.Server.Data.Models;
using LabApp.Shared.Dto.Teacher;
using Microsoft.AspNetCore.SignalR;

namespace LabApp.Server.Hubs
{
	public class TeacherHub : Hub
	{
		public static readonly ConcurrentDictionary<int, string> Users = new ConcurrentDictionary<int, string>();
			
		public TeacherHub()
		{
		}

		public async Task NewAssignment(int assignmentId, int studentAssignmentId, int userId)
		{
			await Clients.User(userId.ToString()).SendAsync(nameof(NewAssignment), assignmentId, studentAssignmentId);
		}
	}
	
	public class HubUserIdProvider : IUserIdProvider
	{
		public virtual string GetUserId(HubConnectionContext connection)
		{
			return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}