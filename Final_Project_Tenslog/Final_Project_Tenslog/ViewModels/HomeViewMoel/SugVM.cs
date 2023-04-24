using Final_Project_Tenslog.Models;

namespace Final_Project_Tenslog.ViewModels.HomeViewMoel
{
    public class SugVM
    {
        public IEnumerable<AppUser> Suggestions{ get; set; }

        public AppUser MyProfile { get; set; }
    }
}
