using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.AcconutViewModel;
using Final_Project_Tenslog.ViewModels.UserProfileViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public ProfileController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;

        }
        [HttpGet]
        public async Task<IActionResult> MyProfile() 
        {
            AppUser appUser = await _userManager.Users
                .Include(u=>u.Followers)
                .Include(u=>u.Followings)
                .Include(u=>u.Posts.Where(p=>p.IsDeleted == false).OrderBy(u => u.CreatedAt))
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            return View(appUser);
        }
        [HttpGet]
        public async Task<IActionResult> UserProfile(string? id)
        {
            UserProfileVM userVM = new UserProfileVM
            {
                User = await _userManager.Users
                .Include(u => u.Followers)
                .Include(u=>u.Followings)
                .Include(u => u.Posts.Where(p => p.IsDeleted == false).OrderBy(u => u.CreatedAt))
                .FirstOrDefaultAsync(u => u.Id == id),
                MyProfile = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name)
        };
            return View(userVM);
        }
        [HttpGet]
        public async Task<IActionResult> Follow(string? id)
        { 
            AppUser following = await _context.Users
                .Include(u => u.Followings)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u=> u.Id == id);

            AppUser follower = await _context.Users
                .Include(u => u.Followings)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);



            await _context.SaveChangesAsync();

            return RedirectToAction("UserProfile","Profile", new { id = id });
        }
    }
}
