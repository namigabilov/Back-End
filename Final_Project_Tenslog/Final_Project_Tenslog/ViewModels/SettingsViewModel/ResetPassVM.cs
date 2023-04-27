using System.ComponentModel.DataAnnotations;

namespace Final_Project_Tenslog.ViewModels.SettingsViewModel
{
    public class ResetPassVM
    {
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
