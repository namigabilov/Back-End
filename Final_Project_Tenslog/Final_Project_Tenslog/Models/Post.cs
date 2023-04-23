﻿using System.ComponentModel.DataAnnotations;

namespace Final_Project_Tenslog.Models
{
    public class Post : BaseEntity
    {
        public string ImageUrl { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public AppUser? User { get; set; }
        public string? UserId { get; set; }

        public IEnumerable<Like>? Likes { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
        public IEnumerable<Saved>? Saved { get; set; }
    }
}