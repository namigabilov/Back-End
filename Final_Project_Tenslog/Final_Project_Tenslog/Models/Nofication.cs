using Final_Project_Tenslog.Enums;

namespace Final_Project_Tenslog.Models
{
    public class Nofication : BaseEntity
    {
        public bool IsRead { get; set; } = false;

        public NoficationType NoficationType{ get; set; }

        public AppUser User { get; set; }

        public string UserId { get; set; }

        public Post Post { get; set; }

        public int? PostId { get; set; }
    }
}
