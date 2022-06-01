using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootyPundits.DTO
{
    public class AccMessageDTO
    {
        public int AccountId { get; set; }
        public int GameId { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public string AccountName { get; set; }
        public string ProfilePath { get; set; }
    }
}
