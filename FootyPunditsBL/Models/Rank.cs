using System;
using System.Collections.Generic;

#nullable disable

namespace FootyPunditsBL.Models
{
    public partial class Rank
    {
        public Rank()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        public int RankId { get; set; }
        public int MinUpvotes { get; set; }
        public string RankName { get; set; }
        public string RankLogo { get; set; }

        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
