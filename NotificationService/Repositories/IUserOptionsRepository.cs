using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationService.DAL.Models;
using NotificationService.Models.Enums;

namespace NotificationService.Repositories
{
    public interface IUserOptionsRepository
    {
        Task<UserNotificationOptions> GetUserOptionsAsync(int userId, NotifierType type);
        Task<IEnumerable<UserNotificationOptions>> GetUserOptionsAsync(int userId);
        Task<IEnumerable<UserNotificationOptions>> GetUserOptionsAsync(IEnumerable<int> userIds, NotifierType type);
    }
}