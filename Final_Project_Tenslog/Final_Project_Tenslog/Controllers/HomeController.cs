using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.HomeViewMoel;
using Final_Project_Tenslog.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            PostsVM postsVM = new PostsVM
            {
                Posts = await _context.Posts
                .Include(c=>c.Likes.Where(l=>l.IsDeleted == false))
                .Include(c=>c.Comments.Where(cm=>cm.IsDeleted == false ))
                .Include(p=>p.Saved)
                .Include(u => u.User)
                .ThenInclude(b => b.Followings)
                .Include(u => u.User)
                .ThenInclude(b => b.Followers)
                .OrderBy(c => c.CreatedAt)
                .Where(p => p.IsDeleted == false).OrderByDescending(c=>c.CreatedAt).ToListAsync(),
                MyProfile = await _userManager.Users.Include(u=>u.Nofications).ThenInclude(c=>c.FromUser).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name)
            };
            SugVM sugVM = new SugVM
            {
                Suggestions = await _context.Users.Where(u=>u.UserName != User.Identity.Name).Include(u=>u.Followers).Take(4).ToListAsync(),
                MyProfile = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name)
            };
            HomeVM homeVM = new HomeVM
            {
                Posts = postsVM,
                Users = sugVM,
                MyProfile = await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName == User.Identity.Name)

            };

            return View(homeVM);
        }
    }
}
