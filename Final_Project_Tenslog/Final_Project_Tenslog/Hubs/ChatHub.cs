using Microsoft.AspNetCore.SignalR;

namespace Final_Project_Tenslog.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.Client(user).SendAsync("ReceiveMessage", user, message);
        }
    }
}
