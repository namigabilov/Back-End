using Final_Project_Tenslog.Enums;

namespace Final_Project_Tenslog.Models
{
    public class VerificationRequest
    {
        public int Id { get; set; }

        public AppUser User { get; set; }

        public string UserId { get; set; }

        public bool Accepted { get; set; } = false;
    }
}
