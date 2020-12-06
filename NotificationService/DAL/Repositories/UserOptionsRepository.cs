using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotificationService.DAL.Models;
using NotificationService.Models.Enums;
using NotificationService.Repositories;

namespace NotificationService.DAL.Repositories
{
    public class UserOptionsRepository : IUserOptionsRepository
    {
        private readonly NotificationDbContext _db;
        
        public UserOptionsRepository(NotificationDbContext db)
        {
            _db = db;
        }

        public async Task<UserNotificationOptions> GetUserOptionsAsync(int userId, NotifierType type)
        {
            return await _db.UserOptions.SingleOrDefaultAsync(x => x.UserId == userId && type == x.Type);
        }

        public async Task<IEnumerable<UserNotificationOptions>> GetUserOptionsAsync(int userId)
        {
            return await _db.UserOptions.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserNotificationOptions>> GetUserOptionsAsync(IEnumerable<int> userIds, NotifierType type)
        {
            return await _db.UserOptions.Where(x => userIds.Contains(x.UserId) && type == x.Type).ToListAsync();
        }
    }
}