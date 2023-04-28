using Final_Project_Tenslog.Enums;

namespace Final_Project_Tenslog.Models
{
    public class Support : BaseEntity
    {
        public AppUser? User { get; set; }

        public string? UserId { get; set; }

        public SupportTitles SupportTitle { get; set; }

        public string Description { get; set; }

        public bool IsRead { get; set; }
    }
}
