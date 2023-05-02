using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
    public class NoficationsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public NoficationsController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _userManager= userManager;
            _context= context;
        }

        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            IEnumerable<Nofication> nofications = await _context.Nofications.Include(n=>n.Post).Include(u=>u.FromUser).OrderByDescending(n=>n.CreatedAt).Where(n=>n.UserId == appUser.Id).ToListAsync();

            return View(nofications);
        }
    }
}
