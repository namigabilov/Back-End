using Final_Project_Tenslog.Models;

namespace Final_Project_Tenslog.ViewModels.SettingsViewModel
{
    public class SettingsVM
    {
        public AppUser? User { get; set; }

        public ResetPassVM? ResetPass { get; set; }

        public SecurityVM? Security { get; set; }

        public Support? Support { get; set; }

        public VerificationRequest? Request { get; set; }

        public IEnumerable<Support>? Supports { get; set; }
    }
}
