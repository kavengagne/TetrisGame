using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameModel.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        
        [StringLength(30, MinimumLength = 5)]
        public string Username { get; set; }

        [StringLength(250, MinimumLength = 5)]
        public string Password { get; set; }

        [StringLength(40, MinimumLength = 2)]
        public string Country { get; set; }
        
        public DateTime RegisteredDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
