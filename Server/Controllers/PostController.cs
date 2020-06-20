/*using System;
using System.Collections.Generic;
using System.Linq;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Controller = LabApp.Server.Controllers.Internal.Controller;

namespace LabApp.Server.Controllers
{
    public class PostController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _db;
        private readonly ILikeRepository _likeRepository;

        public PostController(IUserRepository userRepository, AppDbContext db, ILikeRepository likeRepository)
        {
            _userRepository = userRepository;
            _db = db;
            _likeRepository = likeRepository;
        }

        // GET
        [Authorize]
        [HttpGet]
        [Route("/Posts/")]
        public IActionResult GetMyPosts()
        {
            var user = _userRepository.GetUser(HttpContext.User);
            ViewBag.User = user;
            List<PostViewModel> posts = GetPostsByUser(user.UserId, user.UserId);
            ViewData["tabId"] = "posts-tab";
            return View("MyPosts", posts);
        }
        
        [HttpGet]
        [Route("/User/{userId:int}/Posts/")]
        public IActionResult GetUserPosts(int userId)
        {
            var user = _userRepository.GetUser(userId);
            int? currentUserId = User.Identity.IsAuthenticated ? _userRepository.GetUser(HttpContext.User).UserId : (int?)null;
            if (user is null) return NotFound();
            ViewBag.User = user;
            var posts = GetPostsByUser(userId, currentUserId);
            ViewData["tabId"] = "posts-tab";
            return View("UserPosts", posts);
        }


        [NonAction]
        private List<PostViewModel> GetPostsByUser(int userId, int? currentUserId)
        {
            var posts = _db.Posts.Where(x => x.UserId == userId)
                .Include(x => x.User)
                .ThenInclude(y => y.Photo)
                .Include(x => x.OriginalPost)
                .ThenInclude(x => x.User)
                .Select(p => new PostViewModel(
                    p,
                    p.User,
                    p.OriginalPost,
                    p.OriginalPost.User,
                    p.UserPostLikes.Count))
                .OrderByDescending(x => x.Datetime)
                .ToList();
            if (currentUserId != null)
            {
                foreach (var post in posts)
                {
                    post.IsLiked = _db.PostLikes.Any(x => x.UserId == currentUserId && x.PostId == post.Id);
                }
            }
            return posts;
        }

        [Route("/Post/{postId}/Like/")]
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Like(int postId)
        {
            var user = _userRepository.GetUser(HttpContext.User);
            var post = _db.Posts.SingleOrDefault(x => x.Id == postId);
            if (post == null) return NotFound();
            if (!_likeRepository.LikePost(post, user.UserId)) return BadRequest();
            _db.SaveChanges();
            return Ok(new {Count=_likeRepository.Count(postId), Action=Url.Action(nameof(UnLike), new {postId=postId})});
        }

        [Route("/Post/{postId}/UnLike/")]
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UnLike(int postId)
        {
            var user = _userRepository.GetUser(HttpContext.User);
            var post = _db.Posts.SingleOrDefault(x => x.Id == postId);
            if (post == null) return NotFound();
            if (!_likeRepository.UnLikePost(post, user.UserId)) return BadRequest();
            _db.SaveChanges();
            return Ok(new {Count=_likeRepository.Count(postId), Action=Url.Action(nameof(Like), new {postId=postId})});
        }

        [HttpGet]
        [Route("/Posts/Add/")]
        [Authorize]
        public IActionResult Add()
        {
            return View(new Post());
        }
        
        [HttpPost]
        [Route("/Posts/Add/")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Post post)
        {
            var user = _userRepository.GetUser(HttpContext.User);
            post.User = user;
            post.Text = post.Text.Trim();
            ModelState.Clear();
            TryValidateModel(post);
            if (ModelState.IsValid)
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var entry = _db.OriginalPosts.Add(post);
                        _db.SaveChanges();
                        var userPost = new Post() {OriginalPost = entry.Entity, User = user};
                        _db.Posts.Add(userPost);
                        _db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction(nameof(GetMyPosts));
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    }
                }
            }

            Response.StatusCode = 400;
            return View(post);
        }
        
        [HttpPost]
        [Route("/Posts/{postId}/")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int postId)
        {
            var user = _userRepository.GetUser(HttpContext.User);
            var post = _db.Posts.SingleOrDefault(x => x.UserId == user.UserId && postId==x.Id);
            if (post == null)
                return BadRequest();
            var comments = _db.Comments.Where(x => x.UserPostId == postId);
            _db.RemoveRange(comments);
            _db.Posts.Remove(post);
            // TODO: Cascade delete
            _db.SaveChanges();
            
            return RedirectToAction(nameof(GetMyPosts));
        }
    }
}*/