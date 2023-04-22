using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

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
        public bool? IsPrivate { get; set; }
        public bool? ActivtyStatusIsVisible { get; set; }
        public bool? HaveBlueTic { get; set; }

        public IEnumerable<Post>? Posts { get; set; }
        public IEnumerable<Saved>? Saveds { get; set; }
        public IEnumerable<AppUser>? Followers { get; set; }
        public IEnumerable<AppUser>? Following { get; set; }
    }
}
