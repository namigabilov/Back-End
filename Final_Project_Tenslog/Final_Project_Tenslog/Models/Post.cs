using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project_Tenslog.Models
{
    public class Post : BaseEntity
    {
        public string? ImageUrl { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
        public AppUser? User { get; set; }
        public string? UserId { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
        public List<Like>? Likes { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Saved>? Saved { get; set; }
    }
}
