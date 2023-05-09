using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;
        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Comment> comments = await _context.Comments.Include(c=>c.User).Include(c=>c.Post).ToListAsync();

            return View(comments);
        }
    }
}
