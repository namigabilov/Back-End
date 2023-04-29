using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.ReelsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
    public class ReelsController : Controller
    {
        private readonly AppDbContext _context;

        public ReelsController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            AppUser user = await _context.Users.FirstOrDefaultAsync(p=>p.UserName == User.Identity.Name);

            IEnumerable<Post> posts = await _context.Posts.Where(p=>p.User.IsPrivate == false).ToListAsync();

                ReelsVM reelsVM = new ReelsVM
                {
                    User = user,
                    Reels = posts,
                };
                return View(reelsVM);

        }
        public async Task<IActionResult> SearchUser(string? search) 
        {
            if (search == null)
            {
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<AppUser> users = await _context.Users.Where(u => u.UserName.ToLower().Contains(search.ToLower()) || u.Name.ToLower().Contains(search.ToLower())).ToListAsync();

            return PartialView("/Views/Reels/_UsersPartial.cshtml", users);
        }
    }
}
