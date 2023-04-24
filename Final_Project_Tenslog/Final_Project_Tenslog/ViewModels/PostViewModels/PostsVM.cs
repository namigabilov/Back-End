using Final_Project_Tenslog.Models;

namespace Final_Project_Tenslog.ViewModels.PostViewModels
{
    public class PostsVM
    {
        public IEnumerable<Post> Posts { get; set; }

        public AppUser MyProfile { get; set; }
    }
}
