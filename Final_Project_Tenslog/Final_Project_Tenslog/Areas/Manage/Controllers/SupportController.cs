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
    public class SupportController : Controller
    {

        private readonly AppDbContext _context;

        public SupportController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<Support> supports = _context.Supports.OrderByDescending(c=>c.CreatedAt).Include(c=>c.User).Where(c=>c.IsRead ==false);
            TempData["page"] = "support";
            return View(PageNatedList<Support>.Create(supports,pageIndex,4));
        }
        [HttpGet]
        public async Task<IActionResult> AnswerSupport(int? id)
        {
            Support support = await _context.Supports.Include(c=>c.User).FirstOrDefaultAsync(c => c.Id == id);

            return View(support);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerSupport(Support support)
        {
            if (!ModelState.IsValid)
            {
                return View(support);
            }
            Support dbSupport = await _context.Supports.FirstOrDefaultAsync(c=>c.Id == support.Id);

            if (dbSupport == null) return NotFound();

            dbSupport.AdminAnswer = support.AdminAnswer;
            dbSupport.IsRead = true;

            await _context.SaveChangesAsync();

            TempData["page"] = "support";
            return RedirectToAction(nameof(Index));
        }
    }
}
