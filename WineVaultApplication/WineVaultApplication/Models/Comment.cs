using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WineVaultApplication.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public string WineId { get; set; }
        public virtual Wine Wine { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}