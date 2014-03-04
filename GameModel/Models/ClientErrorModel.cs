using System;
using System.Collections.Generic;

namespace GameModel.Models
{
    public class ClientErrorModel
    {
        public Exception Exception { get; set; }
        public String Message { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}