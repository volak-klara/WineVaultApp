using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WineVaultApplication.Models
{
    public class WineLike
    {
        public int Id { get; set; }
        public string WineId { get; set; }
        public string UserId { get; set; }
    }
}