using Final_Project_Tenslog.Areas.Manage.ViewModels;
using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace Final_Project_Tenslog.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            IQueryable<AppUser> users = _context.Users.Include(c=>c.Posts).Where(c=>c.EmailConfirmed);
            TempData["page"] = "user";
            return View(PageNatedList<AppUser>.Create(users,pageIndex,4));
        }
    }
}
