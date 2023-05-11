using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.SettingsViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Final_Project_Tenslog.Extentions;
using Microsoft.AspNetCore.SignalR;
using Final_Project_Tenslog.Hubs;

namespace Final_Project_Tenslog.Controllers
{
    public class SettingsController : Controller
    {
        public string sessionId = "";
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<NoficationHub> _hub;
        private readonly IWebHostEnvironment _env;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        public SettingsController(UserManager<AppUser> userManager, AppDbContext context, SignInManager<AppUser> signInManager, IWebHostEnvironment env, IHubContext<NoficationHub> hub)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _env = env;
            _hub = hub;
        }

        public async Task<IActionResult> EditProfile()
        {
            AppUser appUser = await _context.Users
                .Include(c=>c.Nofications)
                .ThenInclude(c=>c.FromUser)
                .Include(c=>c.Nofications)
                .ThenInclude(c=>c.Post)
                .FirstOrDefaultAsync(u=>u.UserName == User.Identity.Name);

            if (appUser == null) return BadRequest();

            VerificationRequest request = await _context.VerificationRequests.FirstOrDefaultAsync(c => c.UserId == appUser.Id);
            

            SecurityVM securityVM = new SecurityVM
            {
                IsPrivate = appUser.IsPrivate,
                ActivtyStatusIsVisible = appUser.ActivtyStatusIsVisible
            };

            SettingsVM settingsVM = new SettingsVM
            {
                User = appUser,
                Security = securityVM,
                Request = request,
                Supports = await _context.Supports.Where(c=>c.IsRead && c.UserId == appUser.Id).ToListAsync()
            };

            return View(settingsVM);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteProfilePhoto(string? id)
        {
            if (id == null) return BadRequest();

            AppUser appUser = _context.Users.FirstOrDefault(U=>U.Id == id);

            if (appUser == null) return NotFound();

            appUser.ProfilePhotoUrl = "DefaultProfilePhoto.png";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EditProfile));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(SettingsVM settings)
        {
            SettingsVM settingsVM = new SettingsVM
            {
                User = settings.User,
            };
            AppUser dbAppUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Form verileri doğru değil.");
                return View("EditProfile", settingsVM);
            }

            if (settings.User.File != null)
            {
                if (settings.User.File.Length /1000000 > 10)
                {
                    ModelState.AddModelError("File", "File Large than 10 mb chose another one !");
                    return View(settingsVM);
                }
                if (dbAppUser.ProfilePhotoUrl != "DefaultProfilePhoto.png")
                {
                    FileHelper.DeleteFile(dbAppUser.ProfilePhotoUrl, _env, "assets", "Photos", "ProfilePhotos");
                }
                dbAppUser.ProfilePhotoUrl = settings.User.File.CreateFileAsync(_env, "assets", "Photos", "ProfilePhotos").Result;
            }

            if (settings.User == null) return BadRequest();

            if (settings.User.Name != null)
            {
                dbAppUser.Name = settings.User.Name.Trim();
            }
            else
            {
                dbAppUser.Name = dbAppUser.Name;
            }
            if (settings.User.SurName != null)
            {
                dbAppUser.SurName = settings.User.SurName.Trim();
            }
            else
            {
                dbAppUser.SurName = dbAppUser.SurName;
            }
            dbAppUser.Bio = settings.User.Bio;

            if ( _context.Users.Any(u=>u.UserName.Trim().ToLower() == settings.User.UserName.Trim().ToLowerInvariant()))
            {
                if (settings.User.UserName != dbAppUser.UserName)
                {
                    ModelState.AddModelError("", $"{settings.User.UserName} Already Taken Choose Another One");
                    return View("EditProfile", settingsVM);
                }
                else
                {
                    dbAppUser.UserName = settings.User.UserName.Trim();
                }
            }
            else
            {
                dbAppUser.UserName = settings.User.UserName.Trim();
            }

            if (_context.Users.Any(u => u.Email.Trim().ToLower() == settings.User.Email.Trim().ToLowerInvariant()))
            {
                if (settings.User.Email != dbAppUser.Email)
                {
                    ModelState.AddModelError("", "Account email cannot be changed");
                    return View("EditProfile", settingsVM);
                }
                else
                {
                    dbAppUser.Email = settings.User.Email.Trim();
                }
            }
            else
            {
                dbAppUser.Email = settings.User.Email.Trim();
            }
            dbAppUser.PhoneNumber = settings.User.PhoneNumber;

            IdentityResult identityResult = await _userManager.UpdateAsync(dbAppUser);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
                return View("EditProfile", settingsVM);
            }

            await _signInManager.RefreshSignInAsync(dbAppUser);

            return RedirectToAction("myprofile","profile");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(SettingsVM settings)
        {
            SettingsVM settingsVM = new SettingsVM
            {
                User = settings.User,
            };
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill !");
                return View("EditProfile", settingsVM);
            }
            AppUser appUser = await _context.Users.FirstOrDefaultAsync(p=>p.UserName == User.Identity.Name);

            if (appUser == null) { return BadRequest(); }

            if (!string.IsNullOrWhiteSpace(settings.ResetPass.OldPassword))
            {
                if (!await _userManager.CheckPasswordAsync(appUser,settings.ResetPass.OldPassword))
                {
                    ModelState.AddModelError("", "Şifrə Yanlışdır !");
                    return View("EditProfile", settingsVM);
                }
                string token =await _userManager.GeneratePasswordResetTokenAsync(appUser);
                IdentityResult identityResult = await _userManager.ResetPasswordAsync(appUser, token, settings.ResetPass.Password);

                if (!identityResult.Succeeded)
                {
                    foreach (IdentityError identityError in identityResult.Errors)
                    {
                        ModelState.AddModelError("", identityError.Description);
                    }
                    return View("EditProfile", settingsVM);
                }
            }
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login","acconut");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Security(SettingsVM settings)
        {
            AppUser appUser = await _context.Users.FirstOrDefaultAsync(p => p.UserName == User.Identity.Name);
            SecurityVM securityVM = new SecurityVM
            {
                IsPrivate = appUser.IsPrivate,
                ActivtyStatusIsVisible = appUser.ActivtyStatusIsVisible
            };

            SettingsVM settingsVM = new SettingsVM
            {
                User = settings.User,
                Security= securityVM,
            };
            appUser.IsPrivate = settings.Security.IsPrivate;
            appUser.ActivtyStatusIsVisible = settings.Security.ActivtyStatusIsVisible;

            await _userManager.UpdateAsync(appUser);

            return RedirectToAction("MyProfile", "Profile");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Support(SettingsVM settings)
        {
            SettingsVM settingsVM = new SettingsVM
            {
                User = settings.User,
                Security = settings.Security,
            };
            if (!ModelState.IsValid)
            {
                return View("EditProfile", settingsVM);
            }

            if (settings.Support == null) return BadRequest();

            AppUser appUser = await _context.Users.Include(u=>u.Supports).FirstOrDefaultAsync(p => p.UserName == User.Identity.Name);

            Final_Project_Tenslog.Models.Support support = new Support
            {
                Description = settings.Support.Description,
                SupportTitle = settings.Support.SupportTitle,
                IsDeleted = false,
                UserId = appUser.Id,
                CreatedAt = DateTime.UtcNow.AddHours(4)
            };

            appUser.Supports.Add(support);

            await _context.SaveChangesAsync();

            _hub.Clients.Group("admins").SendAsync("ReciveNotify", $"{appUser.Name} {appUser.SurName} Have a new Question ");

            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerificationAdded(string? id)
        {
            AppUser appUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);

            if (appUser == null )
            {
                return NotFound();
            }

            if (appUser.Request != null)
            {
                return RedirectToAction(nameof(EditProfile));
            }
            VerificationRequest request = new VerificationRequest
            {
                UserId = appUser.Id,
                Accepted = false
            };

            appUser.Request = request;

            await _context.VerificationRequests.AddAsync(request);

            await _context.SaveChangesAsync();

            _hub.Clients.Group("admins").SendAsync("ReciveNotify",$"{appUser.Name} {appUser.SurName} Send a new Verification Requests");

            return RedirectToAction("MyProfile","Profile");
        }

    }
    
}
