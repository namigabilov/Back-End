﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Microsoft.AspNetCore.Identity;
using Final_Project_Tenslog.ViewModels.DirectViewModels;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
    public class DirectController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public DirectController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AppUser appUser =await _userManager.FindByNameAsync(User.Identity.Name);

            List<MyDirect> myDirect = await _context.MyDirects
                .Include(c=>c.WriteingWithUser)
                .Include(c=>c.AppUser)
                .Include(c=>c.Messages)
                .Where(c=>c.WriteingWithUserId == appUser.Id || c.AppUserId == appUser.Id).ToListAsync();

            ViewBag.MyId = $"{appUser.Id}";

            DirectVM directVM = new DirectVM    
            {
                MyProfile = appUser,
                MyDirects = myDirect,
            };

            return View(directVM);
        }
        [HttpGet]
        public async Task<IActionResult> ChatBox(string? id)
        {
            AppUser MyProfile = await _userManager.FindByNameAsync(User.Identity.Name);

            AppUser appUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);

            MyDirect myDirect = await _context.MyDirects.Include(c => c.Messages).FirstOrDefaultAsync(c => (c.WriteingWithUserId == MyProfile.Id && c.AppUserId == appUser.Id) || (c.WriteingWithUserId == appUser.Id && c.AppUserId == MyProfile.Id));

            List<Message> messages = myDirect.Messages;

            ViewBag.MyId = $"{MyProfile.Id}";

            ChatBoxVM chatBoxVM = new ChatBoxVM
            {
                MyProfile = MyProfile,
                UserProfile = appUser,
                Messages = messages,
            };

            return View(chatBoxVM);
        }
        public async Task<IActionResult> SendMessage(MessageVM writedMessage,string? id)
        {
            AppUser MyProfile = await _userManager.FindByNameAsync(User.Identity.Name);

            AppUser user = await _userManager.FindByNameAsync(id);

            if (_context.MyDirects.Any(c=>(c.WriteingWithUserId == MyProfile.Id && c.AppUserId == user.Id) || (c.WriteingWithUserId == user.Id && c.AppUserId == MyProfile.Id)))
            {
                MyDirect myDirect = await _context.MyDirects.FirstOrDefaultAsync(c=>(c.WriteingWithUserId == MyProfile.Id && c.AppUserId == user.Id) || (c.WriteingWithUserId == user.Id && c.AppUserId == MyProfile.Id));

                Message message = new Message
                {
                    MessageContent = writedMessage.Message,
                    MyDirectId = myDirect.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(4),
                    WhoWrite = MyProfile.Id
                };

                _context.Messages.Add(message);
            }
            else
            {
                MyDirect direct = new MyDirect
                {
                    AppUserId = MyProfile.Id,
                    WriteingWithUserId = user.Id
                };

                _context.MyDirects.Add(direct);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        public IActionResult MobileChat()
        {
            return View();
        }
    }
}
