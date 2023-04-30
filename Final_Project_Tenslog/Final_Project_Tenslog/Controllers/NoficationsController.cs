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

        public IActionResult Index()
        {
            return View();
        }
    }
}
