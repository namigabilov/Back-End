using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Extentions;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public PostController(UserManager<AppUser> userManager, AppDbContext context, IWebHostEnvironment env)
        {
            _userManager = userManager;
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
                MyProfile = await _userManager.FindByNameAsync(User.Identity.Name),
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

            if (post == null) return NotFound();

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == User.Identity.Name);

            bool isLiked = post.Likes.Any(l=>l.UserId == user.Id && l.PostId == post.Id); 

            if (isLiked)
            {
                Final_Project_Tenslog.Models.Like like = post.Likes.FirstOrDefault(p => p.UserId == user.Id && p.PostId == post.Id);

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

                post.Likes.Add(like);
            }
            await _context.SaveChangesAsync();

            return Json(post.Likes.Count());
        }
        [HttpGet]
        public async Task<IActionResult> Save(int? id)
        {
            if (id == null) return BadRequest();

            Post post = await _context.Posts
                .Include(p=>p.Saved)
                .Include(p => p.Likes.Where(l => l.IsDeleted == false)).FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == User.Identity.Name);

            bool isSaved = post.Saved.Any(s => s.UserId == user.Id && s.PostId == post.Id);

            if (isSaved)
            {
                Saved saved = post.Saved.FirstOrDefault(s => s.UserId == user.Id && s.PostId == post.Id);

                post.Saved.Remove(saved);
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

            if (post == null) return NotFound();

            Comment dbComment = new Comment
            {
                UserId = appUser.Id,
                PostId = post.Id,
                Description= comment.Description,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                CreatedBy = $"{appUser.Name} {appUser.SurName}",
            };

            post.Comments.Add(dbComment);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id = id });
        }
        [HttpGet]
        public async Task<IActionResult> AddPost()
        {
            return View();
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

            _context.Comments.RemoveRange(comments);
            _context.Likes.RemoveRange(likes);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
