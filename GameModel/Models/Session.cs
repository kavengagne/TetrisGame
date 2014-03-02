using System;

namespace GameModel.Models
{
    public class Session
    {
        public int SessionID { get; set; }
        public int UserID { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}