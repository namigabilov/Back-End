using Final_Project_Tenslog.Models;
using Final_Project_Tenslog.ViewModels.PostViewModels;

namespace Final_Project_Tenslog.ViewModels.HomeViewMoel
{
    public class HomeVM
    {
        public SugVM Users { get; set; }

        public PostsVM Posts { get; set; }

        public AppUser MyProfile { get; set; }

    }
}
