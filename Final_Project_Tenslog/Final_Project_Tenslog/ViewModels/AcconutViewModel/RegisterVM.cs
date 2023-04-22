using System.ComponentModel.DataAnnotations;

namespace Final_Project_Tenslog.ViewModels.AcconutViewModel
{
    public class RegisterVM
    {
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
