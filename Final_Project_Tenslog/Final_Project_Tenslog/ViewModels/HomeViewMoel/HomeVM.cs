using Final_Project_Tenslog.Models;

namespace Final_Project_Tenslog.ViewModels.HomeViewMoel
{
    public class HomeVM
    {
        public IEnumerable<AppUser> Users { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        public AppUser MyProfile { get; set; }

    }
}
