using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace GameModel.Models.Database
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        
        [MinLength(5), MaxLength(30)]
        public string Username { get; set; }
        
        [MinLength(5), MaxLength(250)]
        public string Password { get; set; }
        
        [MinLength(2), MaxLength(40)]
        public string Country { get; set; }
        
        public DateTime RegisteredDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        [NotMapped]
        public int SessionID { get; set; }
    }
}
