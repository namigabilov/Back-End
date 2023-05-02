using System.ComponentModel.DataAnnotations;

namespace Final_Project_Tenslog.ViewModels.AcconutViewModel
{
    public class RefleshPassword
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfrimPassword { get; set; }
    }
}
