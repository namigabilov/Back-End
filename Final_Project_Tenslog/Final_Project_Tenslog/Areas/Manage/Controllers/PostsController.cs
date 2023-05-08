using Final_Project_Tenslog.Areas.Manage.ViewModels;
using Final_Project_Tenslog.DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class PostsController : Controller
    {

        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            PostVM vm = new PostVM
            {
                Posts = await _context.Posts
                .Include(c=>c.User)
                .Include(c=>c.Likes)
                .Include(c=>c.Comments)
                .Where(c => !c.IsDeleted).OrderByDescending(c=>c.CreatedAt).ToListAsync()
            };

            return View(vm);
        }
    }
}
