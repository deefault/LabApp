using Social.Models;

namespace SocialNetwork.Models.ViewModels
{
    public class UserWithFriend
    {
        public UserWithFriend(User user, bool isFriend=false)
        {
            User = user;
            IsFriend = isFriend;
        }

        public User User { get; set; }

        public bool IsFriend { get; set; } = false;
    }
}