using System;
using System.Collections.Generic;

#nullable disable

namespace FootyPunditsBL.Models
{
    public partial class PlayerRating
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public int PlayerRating1 { get; set; }
        public int AccountId { get; set; }

        public virtual UserAccount Account { get; set; }
    }
}
