using Final_Project_Tenslog.Areas.Manage.ViewModels;
using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels;
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

        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            IQueryable<Post> posts = _context.Posts
                 .Include(c => c.User)
                 .Include(c => c.Likes)
                 .Include(c => c.Comments)
                 .Where(c => !c.IsDeleted).OrderByDescending(c => c.CreatedAt);
            TempData["page"] = "post";
            return View(PageNatedList<Post>.Create(posts,pageIndex,4));
        }
    }
}
