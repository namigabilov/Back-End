using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Hubs;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.AcconutViewModel;
using Final_Project_Tenslog.ViewModels.UserProfileViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IHubContext<NoficationHub> _hub;

        public ProfileController(UserManager<AppUser> userManager, AppDbContext context,IHubContext<NoficationHub> hub)
        {
            _userManager = userManager;
            _context = context;
            _hub = hub;

        }
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            AppUser appUser = await _userManager.Users
                .Include(c=>c.Saveds)
                .Include(u=>u.Nofications)
                .ThenInclude(c=>c.Post)
                .Include(u => u.Nofications)
                .ThenInclude(c => c.FromUser)
                .Include(u=>u.Saveds)
                .ThenInclude(s=>s.Post)
                .Include(u => u.Followers)
                .ThenInclude(c=>c.UserFollower)
                .Include(u => u.Followings)
                .ThenInclude(c=>c.UserFollowing)
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
                .Include(c=>c.Saveds)
                .Include(u => u.Nofications)
                .ThenInclude(c => c.Post)
                .Include(u => u.Nofications)
                .ThenInclude(c => c.FromUser)
                .Include(u => u.Followers)
                .Include(u => u.Followings)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name)
            };

            if (userVM.User.Id == userVM.MyProfile.Id)
            {
                return RedirectToAction(nameof(MyProfile));
            }

            return View(userVM);
        }
        [HttpGet]
        public async Task<IActionResult> RejectFollow(string? id)
        {

            AppUser appUser = await _context.Users.Include(u=>u.Nofications).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            Nofication nofication = _context.Nofications.FirstOrDefault(u => u.FromUserId == id && u.UserId == appUser.Id);
            _context.Nofications.Remove(nofication);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public async Task<IActionResult> AcceptFollow(string? id)
        {
            AppUser follower = await _context.Users
                .Include(u => u.Nofications)
                .Include(u => u.Followings)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == id);

            AppUser following = await _context.Users
                .Include(u=>u.Nofications)
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

            Nofication nofication = _context.Nofications.FirstOrDefault(u => u.UserId == following.Id && u.FromUserId == id);
            

            _context.Nofications.Remove(nofication);

            await _context.SaveChangesAsync();

            return RedirectToAction("UserProfile", "Profile", new { id = id });
        }
        [HttpGet]
        public async Task<IActionResult> Follow(string? id)
        {
            AppUser following = await _context.Users
                .Include(u=>u.Nofications)
                .Include(u => u.Followings)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == id);

            AppUser follower = await _context.Users
                .Include(u => u.Nofications)
                .Include(u => u.Followings)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (following.IsPrivate == false)
            {
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


                _hub.Clients.User(following.Id).SendAsync("ReciveNotifyForFollow", "fa-user");

            }
            else
            {
                _hub.Clients.User(following.Id).SendAsync("ReciveNotifyForFollow", "fa-user");
            }

            if (!following.Nofications.Any(c=>c.FromUserId == follower.Id))
            {
                Nofication nofication = new Nofication
                {
                    NoficationType = (Enums.NoficationType)2,
                    UserId = follower.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(4),
                    IsRead = false,
                    IsDeleted = false,
                    FromUserId = follower.Id,
                };

                following.Nofications.Add(nofication);
            }

            

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
            follower.Followings.Remove(follower.Followings.FirstOrDefault(f=>f.UserId == follower.Id));
            
            
            await _context.SaveChangesAsync();

            return RedirectToAction("UserProfile", "Profile", new { id = id });
        }
    }
}
