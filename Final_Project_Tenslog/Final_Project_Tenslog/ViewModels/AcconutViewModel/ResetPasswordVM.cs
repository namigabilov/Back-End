using System.ComponentModel.DataAnnotations;

namespace Final_Project_Tenslog.ViewModels.AcconutViewModel
{
    public class ResetPasswordVM
    {
        [EmailAddress]
        public string Email { get; set; }

    }
}
