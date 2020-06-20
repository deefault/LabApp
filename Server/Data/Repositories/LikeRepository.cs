using System.Linq;
using LabApp.Server.Data.Models;
using LabApp.Server.Data;


namespace LabApp.Server.Data.Repositories
{
    public interface ILikeRepository
    {
        bool IsLiked(Post post, int userId);

        bool LikePost(Post post, int userId);

        bool UnLikePost(Post post, int userId);

        int Count(int postId);
    }

    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _db;

        public LikeRepository(AppDbContext db)
        {
            _db = db;
        }

        public bool IsLiked(Post post, int userId)
        {
            return _db.PostLikes.SingleOrDefault(x => x.UserId == userId && post.Id == x.PostId) != null;
        }

        public bool LikePost(Post post, int userId)
        {
            if (IsLiked(post, userId)) return false;
            _db.PostLikes.Add(new PostLike {PostId = post.Id, UserId = userId});
            return true;
        }

        public bool UnLikePost(Post post, int userId)
        {
            var like = _db.PostLikes.SingleOrDefault(x => x.UserId == userId && post.Id == x.PostId);
            if (like == null) return false;
            _db.PostLikes.Remove(like);
            return true;
        }

        public int Count(int postId)
        {
            return _db.PostLikes.Count(x => x.PostId == postId);
        }
    }
}