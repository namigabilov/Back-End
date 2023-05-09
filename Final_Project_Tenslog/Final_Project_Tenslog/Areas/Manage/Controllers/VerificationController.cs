using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Identity;

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
        public async Task<IActionResult> Index()
        {
            IEnumerable<VerificationRequest> requests = await _context.VerificationRequests
                .Include(c=>c.User)
                .Where(c => c.Accepted == false).ToListAsync();

            return View(requests);
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
