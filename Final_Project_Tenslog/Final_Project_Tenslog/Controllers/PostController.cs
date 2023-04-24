using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public PostController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;

        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            PostVM postVM = new PostVM
            {
                MyProfile = await _userManager.FindByNameAsync(User.Identity.Name),
                Post = await _context.Posts
                .Include(p=>p.Comments.Where(c=>c.IsDeleted== false))
                .Include(p=>p.Likes.Where(pd=>pd.IsDeleted == false))
                .Include(p=>p.User).FirstOrDefaultAsync(p=>p.Id == id)
               
            };

            return View(postVM);
        }
        [HttpGet]
        public async Task<IActionResult> Like(int? id)
        {
            Post post = await _context.Posts.Include(p=>p.Likes.Where(l=>l.IsDeleted == false)).FirstOrDefaultAsync(p => p.Id == id);

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
            return RedirectToAction(nameof(Index), new { id = id});
        }
    }
}
