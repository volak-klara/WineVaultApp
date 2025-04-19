using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WineVaultApplication.Models
{
    public class Wine
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "The name of the wine is required.")]
        [MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A description is required.")]

        public string Description { get; set; }

        [Required]
        public string Category { get; set; } // Red, Rose, Sparkling, White

        public string ImagePath { get; set; } 

        public string UserId { get; set; } 

        public virtual ApplicationUser User { get; set; }

        public int LikesCount { get; set; } = 0;

        public virtual ICollection<Comment> Comments { get; set; }

        public DateTime CreatedAt { get; set; }
       
        [NotMapped] 
        public bool IsLikedByCurrentUser { get; set; }

    }
}