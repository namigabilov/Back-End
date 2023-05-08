using Final_Project_Tenslog.Areas.Manage.ViewModels;
using Final_Project_Tenslog.DataAccessLayer;
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

        public async Task<IActionResult> Index()
        {
            UsersVM vm = new UsersVM
            {
                Users = await _context.Users.Include(c => c.Posts).Where(c => c.EmailConfirmed == true).ToListAsync()
            };
            return View(vm);
        }
    }
}
