using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels;
using Final_Project_Tenslog.ViewModels.AcconutViewModel;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Final_Project_Tenslog.Controllers
{
    public class AcconutController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly SmtpSetting _smtpSetting;
        private readonly AppDbContext _context;


        public AcconutController(IOptions<SmtpSetting> operation, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _smtpSetting = operation.Value;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;

        }
        [HttpGet]
        public async Task<IActionResult> ResetPass()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPass(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordVM);
            }
            AppUser appUser = await _userManager.FindByEmailAsync(resetPasswordVM.Email);

            if (appUser == null)
            {
                ModelState.AddModelError("", $"{resetPasswordVM.Email} Emaile Malik User Tapilmadi !");
                return View(resetPasswordVM);
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

            string url = Url.Action("ResetPassword", "Acconut", new { id = appUser.Id, token = token }, HttpContext.Request.Scheme, HttpContext.Request.Host.ToString());

            string tempalteFullPath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "_FogetPasswordPartial.cshtml");

            string templateContent = await System.IO.File.ReadAllTextAsync(tempalteFullPath);
            
            templateContent = templateContent.Replace("{{url}}", url);

            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(_smtpSetting.Email));
            mimeMessage.To.Add(MailboxAddress.Parse(appUser.Email));
            mimeMessage.Subject = "Forget Password";
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = templateContent
            };
            using (SmtpClient smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_smtpSetting.Host, _smtpSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_smtpSetting.Email, _smtpSetting.Password);
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);
                smtpClient.Dispose();
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string token, RefleshPassword resetPasswordVM)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return BadRequest();
            }
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(appUser, token, resetPasswordVM.Password);
            if (!identityResult.Succeeded)
            {
                return BadRequest();
            }
            return RedirectToAction("Login", "Acconut");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser appUser = new AppUser
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                Name = registerVM.Name,
                JoinedDate = DateTime.UtcNow.AddHours(4),
                Gender = "Perfer Not To Say",
                IsPrivate = false,
                HaveBlueTic = false,
                ProfilePhotoUrl = "DefaultProfilePhoto.png"
            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(appUser, "Member");

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            string url = Url.Action("EmailConfirm", "Acconut", new { id = appUser.Id, token = token }, HttpContext.Request.Scheme, HttpContext.Request.Host.ToString());

            string tempalteFullPath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "_EmailConfrimPartial.cshtml");

            string templateContent = await System.IO.File.ReadAllTextAsync(tempalteFullPath);

            templateContent = templateContent.Replace("{{name}}", appUser.Name);
            templateContent = templateContent.Replace("{{surname}}", appUser.SurName);
            templateContent = templateContent.Replace("{{url}}", url);


            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(_smtpSetting.Email));
            mimeMessage.To.Add(MailboxAddress.Parse(appUser.Email));
            mimeMessage.Subject = "Email Confirm";
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = templateContent
            };
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.CheckCertificateRevocation = false;
                await smtpClient.ConnectAsync(_smtpSetting.Host, _smtpSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_smtpSetting.Email, _smtpSetting.Password);
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);
                smtpClient.Dispose();
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task<IActionResult> EmailConfirm(string id, string token)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return BadRequest();
            }
            IdentityResult identityResult = await _userManager.ConfirmEmailAsync(appUser, token);
            if (!identityResult.Succeeded)
            {
                return BadRequest();
            }
            await _signInManager.SignInAsync(appUser, false);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == loginVM.Email.Trim().ToUpperInvariant());


            if (appUser == null && !(await _userManager.CheckPasswordAsync(appUser, loginVM.Password)))
            {
                ModelState.AddModelError("", "Email ve ya Sifre Yanlisdir");
                return View(loginVM);
            }

            if (appUser.EmailConfirmed)
            {
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager
               .PasswordSignInAsync(appUser, loginVM.Password, loginVM.RemindMe, true);
                if (signInResult.IsLockedOut)
                {

                    ModelState.AddModelError("", $"Hesabiniz Blokalnib Hesabin Acilma Tarixi : {appUser.LockoutEnd?.ToString("dd MMMM yyyy HH:mm")} (+4 Utc)");
                    return View(loginVM);
                }
                if (!signInResult.Succeeded)
                {
                    ModelState.AddModelError("", $"Email ve ya Sifre Yanlisdir Duzgun Sifreni Daxil Etmek Ucun Son {3 - appUser.AccessFailedCount} Haqqiniz Qalib !!");
                    return View(loginVM);
                }
            }
            else
            {
                ModelState.AddModelError("", "Email Tesdiq Olunmayib LogIn Prosesi Uğursuzdur !");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }
        #region CreateAdminAndRoles
        //[HttpGet]
        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    return Content("Ugurlu");
        //}
        //[HttpGet]
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        Name = "Super",
        //        SurName = "Admin",
        //        UserName = "SuperAdmin",
        //        Email = "superadmin@gmail.com",
        //        ProfilePhotoUrl = "DefaultProfilePhoto.png",
        //        HaveBlueTic = true

        //    };
        //    await _userManager.CreateAsync(appUser, "SuperAdmin123");
        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");
        //    return Content("Ugurlu");
        //}
        #endregion


    }
}
