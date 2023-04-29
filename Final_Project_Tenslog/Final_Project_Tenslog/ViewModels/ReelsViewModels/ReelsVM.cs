using Final_Project_Tenslog.Models;

namespace Final_Project_Tenslog.ViewModels.ReelsViewModels
{
    public class ReelsVM
    {
        public AppUser User { get; set; }

        public IEnumerable<Post> Reels { get; set; }
    }
}
