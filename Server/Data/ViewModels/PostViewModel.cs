using System;
using BlazorApp.Server.Data.Models;

namespace BlazorApp.Server.Data.ViewModels
{
    public class PostViewModel
    {
        public int Id => UserPostId;
        public int PostId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime Datetime { get; set; }
        public int UserPostId { get; set; }
        
        public int UserSharedFromId { get; set; }
        public User UserSharedFrom { get; set; }
        public bool IsReposted => UserSharedFromId == UserId;
        public bool IsLiked { get; set; }
        public int LikeCount { get; set; }
        
        public PostViewModel(Post post, User user, OriginalPost originalPost, User userSharedFrom, int likeCount)
        {
            PostId = originalPost.PostId;
            UserId = post.UserId;
            User = user;
            UserSharedFromId = originalPost.UserId;
            Datetime = originalPost.Datetime;
            Text = originalPost.Text;
            UserPostId = post.OriginalPostId;
            UserSharedFrom = userSharedFrom;
            LikeCount = likeCount;
            IsLiked = false;
        }
    }
}