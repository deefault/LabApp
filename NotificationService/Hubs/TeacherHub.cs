using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Hubs
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
}