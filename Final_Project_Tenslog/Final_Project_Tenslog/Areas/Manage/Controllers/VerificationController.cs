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
    public class VerificationController : Controller
    {
        private readonly AppDbContext _context;

        public VerificationController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            IQueryable<VerificationRequest> requests = _context.VerificationRequests
                .Include(c=>c.User)
                .Where(c => c.Accepted == false);

            IEnumerable<VerificationRequest> verif = _context.VerificationRequests
                .Include(c => c.User)
                .Take(3)
                .Where(c => c.Accepted == false);
            TempData["page"] = "verification";
            return View(PageNatedList<VerificationRequest>.Create(requests,pageIndex,4));
        }
        public async Task<IActionResult> AcceptRequest(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VerificationRequest request = await _context.VerificationRequests.FirstOrDefaultAsync(c => c.UserId == id);

            if (request == null) return NotFound();

            request.Accepted = true;

            AppUser appUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);

            appUser.HaveBlueTic = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RejectRequest(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VerificationRequest request = await _context.VerificationRequests.FirstOrDefaultAsync(c => c.UserId == id);

            if (request == null) return NotFound();

            _context.VerificationRequests.Remove(request);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}
