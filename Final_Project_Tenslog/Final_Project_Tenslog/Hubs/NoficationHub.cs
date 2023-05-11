using Final_Project_Tenslog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.Hubs
{
    public class NoficationHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public NoficationHub(UserManager<AppUser> userManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public override async Task OnConnectedAsync()
        {
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);

                appUser.ConnectionId = Context.ConnectionId;

                await _userManager.UpdateAsync(appUser);

                if (_httpContext.HttpContext.User.IsInRole("SuperAdmin"))
                {
                    await Groups.AddToGroupAsync(appUser.ConnectionId, "admins");
                }
            }   
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.ConnectionId == Context.ConnectionId);

            if (appUser != null)
            {
                appUser.ConnectionId = null;

                await _userManager.UpdateAsync(appUser);
            }
        }
    }
}
