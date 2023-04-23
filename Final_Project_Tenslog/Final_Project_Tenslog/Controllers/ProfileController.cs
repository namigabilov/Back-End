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
                .Include(u => u.Followers)
                .Include(u => u.Followings)
                .Include(u => u.Posts.Where(p => p.IsDeleted == false).OrderBy(u => u.CreatedAt))
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
                .Include(u => u.Followings)
                .Include(u => u.Posts.Where(p => p.IsDeleted == false).OrderBy(u => u.CreatedAt))
                .FirstOrDefaultAsync(u => u.Id == id),
                MyProfile = await _userManager.Users
                .Include(u => u.Followers)
                .Include(u => u.Followings)
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
                .FirstOrDefaultAsync(u => u.Id == id);

            AppUser follower = await _context.Users
                .Include(u => u.Followings)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            Follower followerDb = new Follower
            {
                UserId = following.Id,
                UserFollowerId = follower.Id
            };
            Following followingDb = new Following
            {
                UserId = follower.Id,
                UserFollowingId = following.Id
            };

            follower.Followings.Add(followingDb);
            following.Followers.Add(followerDb);

            await _context.SaveChangesAsync();

            return RedirectToAction("UserProfile", "Profile", new { id = id });
        }
        [HttpGet]
        public async Task<IActionResult> UnFollow(string? id){

            AppUser following = await _context.Users
                .Include(u => u.Followings)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == id);

            AppUser follower = await _context.Users
                .Include(u => u.Followings)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            ;
            following.Followers.Remove(following.Followers.FirstOrDefault(f => f.UserId == id));
            follower.Followings.Remove(follower.Followings.FirstOrDefault(f=>f.UserFollowingId == follower.Id));
            
            
            await _context.SaveChangesAsync();

            return RedirectToAction("UserProfile", "Profile", new { id = id });
        }
    }
}
