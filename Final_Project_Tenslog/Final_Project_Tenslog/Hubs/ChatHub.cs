using Final_Project_Tenslog.DataAccessLayer;
using Final_Project_Tenslog.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly HttpContext _http;

        public ChatHub(AppDbContext context,HttpContext http)
        {
            _context = context;
            _http = http;
        }

        public async Task SendMessage(string userId, string message)
        {
            AppUser user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);

            AppUser myProfile = await _context.Users.FirstOrDefaultAsync(c => c.UserName == _http.User.Identity.Name);

            MyDirect direct = await _context.MyDirects.FirstOrDefaultAsync(c => (c.WriteingWithUserId == user.Id && c.AppUserId == myProfile.Id) || c.WriteingWithUserId == myProfile.Id && c.AppUserId == user.Id);

            Message dbMessage = new Message
            {
                MessageContent = message,
                MyDirectId = direct.Id,
                CreatedAt= DateTime.UtcNow.AddHours(4),
                WhoWrite = myProfile.Id
            };
            await _context.Messages.AddAsync(dbMessage);

            bool myMesasge = true;

            if(userId == myProfile.Id)
            {
                myMesasge = false;
            }
            string time = DateTime.UtcNow.AddHours(4).ToString("HH:mm");

            await _context.SaveChangesAsync();

            await Clients.Client(user.ConnectionId).SendAsync("ReceiveMessage",time, myMesasge, message);

        }
    }
}
