using System.Collections.Generic;
using Social.Models;

namespace SocialNetwork.Models.ViewModels
{
    public class FriendListViewModel
    {
        public List<User> Users { get; set; }
        public string SearchString { get; set; }
    }
}

