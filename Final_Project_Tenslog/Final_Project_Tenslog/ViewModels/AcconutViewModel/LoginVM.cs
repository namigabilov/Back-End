using System.ComponentModel.DataAnnotations;

namespace Final_Project_Tenslog.ViewModels.AcconutViewModel
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RemindMe { get; set; }
    }
}
