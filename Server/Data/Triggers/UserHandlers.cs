using LabApp.Server.Data.Models;
using EntityFrameworkCore.Triggers;

namespace LabApp.Server.Data.Triggers
{
    public static class UserHandlers
    {
        static UserHandlers()
        {
            Triggers<User>.Inserting += entry =>
            {
                if (entry.Entity.MainPhoto != null || entry.Entity.PhotoId != null)
                    entry.Entity.Thumbnail = entry.Entity.MainPhoto.ThumbnailUrl;
            };

            Triggers<User>.Updating += entry =>
            {
                var user = entry.Entity;
                entry.Context.Entry(user).Reference(x => x.MainPhoto).Load();
                if (user.MainPhoto != null) user.Thumbnail = user.MainPhoto.ThumbnailUrl;
            };
        }
    }
}