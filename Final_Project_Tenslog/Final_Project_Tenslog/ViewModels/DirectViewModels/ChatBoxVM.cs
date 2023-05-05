using Final_Project_Tenslog.Models;

namespace Final_Project_Tenslog.ViewModels.DirectViewModels
{
    public class ChatBoxVM
    {
        public AppUser MyProfile { get; set; }

        public AppUser UserProfile { get; set; }

        public List<Message> Messages { get; set; }
    }
}
