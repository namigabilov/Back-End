using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.HomeViewMoel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Controllers
{
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

            HomeVM homeVM = new HomeVM
            {
                Posts = await _context.Posts.Where(p => p.IsDeleted == false).ToListAsync(),
                Users = await _context.Users.Take(4).ToListAsync(),
                MyProfile = await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName == User.Identity.Name)

            };
            return View(homeVM);
        }
    }
}
