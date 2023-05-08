using Final_Project_Tenslog.Models;

namespace Final_Project_Tenslog.Areas.Manage.ViewModels
{
    public class HomeVM
    {
        public AppUser MyProfile { get; set; }

        public IEnumerable<AppUser> Users { get; set; }

    }
}
