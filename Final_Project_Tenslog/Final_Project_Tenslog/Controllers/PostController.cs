﻿using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Extentions;
using Final_Project_Tenslog.Hubs;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<NoficationHub> _hub;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public PostController(UserManager<AppUser> userManager, AppDbContext context, IWebHostEnvironment env,IHubContext<NoficationHub> hub)
        {
            _userManager = userManager;
            _hub = hub;
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            PostVM postVM = new PostVM
            {
                MyProfile = await _context.Users
                .Include(c=>c.Nofications)
                .ThenInclude(c=>c.FromUser)
                .Include(c=>c.Nofications)
                .ThenInclude(c=>c.Post)
                .FirstOrDefaultAsync(c=>c.UserName == User.Identity.Name),
                Post = await _context.Posts
                .Include(p=>p.Comments.OrderBy(c=>c.CreatedAt).Where(c=>c.IsDeleted== false))
                .ThenInclude(c=>c.User)
                .Include(p=>p.Saved.Where(c=>c.IsDeleted == false))
                .Include(p=>p.Likes.Where(pd=>pd.IsDeleted == false))
                .ThenInclude(u=>u.User)
                .Include(p=>p.User).FirstOrDefaultAsync(p=>p.Id == id)
               
            };

            if (postVM.Post == null || postVM.MyProfile == null)
            {
                return NotFound();
            }

            return View(postVM);
        }
        [HttpGet]
        public async Task<IActionResult> Like(int? id)
        { 
            if (id == null) return BadRequest();

            Post post = await _context.Posts.Include(p=>p.Likes.Where(l=>l.IsDeleted == false)).FirstOrDefaultAsync(p => p.Id == id);

            AppUser postOwner = await _context.Users.Include(p => p.Nofications).FirstOrDefaultAsync(p => p.Id == post.UserId);

            if (post == null) return NotFound();

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == User.Identity.Name);

            bool isLiked = post.Likes.Any(l=>l.UserId == user.Id && l.PostId == post.Id); 

            if (isLiked)
            {
                Final_Project_Tenslog.Models.Like like = post.Likes.FirstOrDefault(p => p.UserId == user.Id && p.PostId == post.Id);

                Nofication nofication = await _context.Nofications.FirstOrDefaultAsync(c => c.PostId == post.Id && c.FromUserId == user.Id && c.UserId == postOwner.Id);

                if (nofication != null)
                {
                    _context.Nofications.Remove(nofication);
                }

                post.Likes.Remove(like);
            }
            else
            {
                Final_Project_Tenslog.Models.Like like = new Like
                {
                    UserId = user.Id,
                    PostId = post.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow.AddHours(4),
                    CreatedBy = $"{user.Name} {user.SurName}",
                };
                Nofication nofication = new Nofication
                {
                    NoficationType = (Enums.NoficationType)1,
                    UserId = postOwner.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(4),
                    IsRead = false,
                    IsDeleted = false,
                    PostId = post.Id,
                    FromUserId = user.Id,
                };
                post.Likes.Add(like);
                postOwner.Nofications.Add(nofication);

                _hub.Clients.User(postOwner.Id).SendAsync("ReciveNotifyForFollow", "fa-heart");
            }

            await _context.SaveChangesAsync();

            return Json(post.Likes.Count());
        }
        [HttpGet]
        public async Task<IActionResult> Save(int? id)
        {
            if (id == null) return BadRequest();

            Post post = await _context.Posts
                .Include(c=>c.Saved)
                .Include(p => p.Likes.Where(l => l.IsDeleted == false)).FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            AppUser user = await _userManager.Users
                .Include(c=>c.Saveds)
                .FirstOrDefaultAsync(p => p.UserName == User.Identity.Name);

            bool isSaved = post.Saved.Any(s => s.UserId == user.Id && s.PostId == post.Id);

            if (isSaved)
            {
                Saved saved = post.Saved.FirstOrDefault(s => s.UserId == user.Id && s.PostId == post.Id);

                user.Saveds.Remove(saved);
            }
            else
            {
                Saved saved = new Saved
                {
                    UserId = user.Id,
                    PostId = post.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow.AddHours(4),
                    CreatedBy = $"{user.Name} {user.SurName}",
                };
                post.Saved.Add(saved);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int? id,Comment comment)
        {
            if (comment == null) return BadRequest();

            if (id == null) return BadRequest();

            AppUser appUser = await _context.Users.FirstOrDefaultAsync(p => p.UserName == User.Identity.Name);

            Post post = await _context.Posts.Include(p=>p.Comments).Where(p=>p.IsDeleted ==false).FirstOrDefaultAsync(p=>p.Id == id);

            AppUser postOwner = await _context.Users.Include(p=>p.Nofications).FirstOrDefaultAsync(p => p.Id == post.UserId);

            if (post == null) return NotFound();

            List<Swears> swears = await _context.Swears.ToListAsync();

            foreach (Swears swear in swears)
            {
                if (comment.Description.ToLowerInvariant().Contains(swear.Words.ToLowerInvariant()))
                {
                    string pattern = swear.Words;
                    string censor = new string('*', swear.Words.Length);
                    comment.Description = Regex.Replace(comment.Description, pattern, censor);
                }
            }

            Comment dbComment = new Comment
            {
                UserId = appUser.Id,
                PostId = post.Id,
                Description= comment.Description,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                CreatedBy = $"{appUser.Name} {appUser.SurName}",
            };

            Nofication nofication = new Nofication
            {
                NoficationType = (Enums.NoficationType)3,
                UserId = postOwner.Id,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                IsRead = false,
                IsDeleted = false,
                PostId = post.Id,
                FromUserId = appUser.Id,
            };

            post.Comments.Add(dbComment);
            postOwner.Nofications.Add(nofication);

            _hub.Clients.User(postOwner.Id).SendAsync("ReciveNotifyForFollow", "fa-comment");

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id = id });
        }
        [HttpGet]
        public async Task<IActionResult> AddPost()
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == User.Identity.Name);
            AddPostVM vM = new AddPostVM
            {
                MyProfile = user
            };
            return View(vM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(Post post)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == User.Identity.Name);

            if (!ModelState.IsValid) return View(post);
            if (post.File == null)
            {
                ModelState.AddModelError("File", "Add Post Please !");
                return View(post);
            }
            else
            {
                if (post.File?.Length / 1000000 > 50 )
                {
                    ModelState.AddModelError("File", "Max Size 50 MB for File !");
                    return View(post);
                }
                if (post.File != null)
                {
                    post.ImageUrl = post.File.CreateFileAsync(_env, "assets", "Photos", "Posts").Result;
                }
                post.CreatedAt = DateTime.UtcNow.AddHours(4);
                post.CreatedBy = $"{user.Name} {user.SurName}";
                post.UserId = user.Id;
            }
            if (post.Description == null)
            {
                post.Description = "";
            }
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync(); 
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return BadRequest();

            Post post = await _context.Posts.FirstOrDefaultAsync(p=>p.Id == id);

            if (post == null) return NotFound();

            FileHelper.DeleteFile(post.ImageUrl, _env, "assets", "Photos", "Posts");

            IEnumerable<Comment> comments = await _context.Comments.Where(c=>c.PostId == post.Id).ToListAsync();
            IEnumerable<Like> likes = await _context.Likes.Where(c => c.PostId == post.Id).ToListAsync() ;
            IEnumerable<Saved> saveds = await _context.Saveds.Where(c=>c.PostId == post.Id).ToListAsync() ;
            IEnumerable<Nofication> nofications = await _context.Nofications.Where(c=>c.PostId == post.Id).ToListAsync() ;
            

            _context.Comments.RemoveRange(comments);
            _context.Likes.RemoveRange(likes);
            _context.Nofications.RemoveRange(nofications);
            _context.Saveds.RemoveRange(saveds);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
