using Final_Project_Tenslog.Areas.Manage.ViewModels;
using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _context.Users.FirstOrDefaultAsync(c => c.UserName == User.Identity.Name);

            IEnumerable<AppUser> users = await _context.Users.Take(3).Include(c=>c.Posts).Where(c=>c.EmailConfirmed == true).ToListAsync();

            HomeVM vm = new HomeVM
            {
                MyProfile = appUser,
                Users = users,
                Posts = await _context.Posts.ToListAsync(),
                Supports = await _context.Supports.ToListAsync(),
            };
            TempData["page"] = "home";
            return View(vm);
        }
    }
}
