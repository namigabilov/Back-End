using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project_Tenslog.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? SurName { get; set; }
        [StringLength(150)]
        public string? Bio { get; set; }
        public Nullable<DateTime> JoinedDate { get; set; }
        public string? Gender { get; set; }
        public bool IsPrivate { get; set; } = false;
        public bool? ActivtyStatusIsVisible { get; set; }
        public bool HaveBlueTic { get; set; } = false;
        public string ProfilePhotoUrl { get; set; }

        public IEnumerable<Post>? Posts { get; set; }
        public IEnumerable<Saved>? Saveds { get; set; }
        public ICollection<Following> Followings { get; set; }
        public ICollection<Follower> Followers { get; set; }

    }
}
