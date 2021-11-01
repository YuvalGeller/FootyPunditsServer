using System;
using System.Collections.Generic;

#nullable disable

namespace FootyPunditsBL.Models
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            AccMessages = new HashSet<AccMessage>();
            PlayerRatings = new HashSet<PlayerRating>();
            VotesHistories = new HashSet<VotesHistory>();
        }

        public int AccountId { get; set; }
        public string AccName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Upass { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsAdmin { get; set; }
        public int FavoriteTeam { get; set; }
        public DateTime SignUpDate { get; set; }
        public int RankId { get; set; }

        public virtual Rank Rank { get; set; }
        public virtual ICollection<AccMessage> AccMessages { get; set; }
        public virtual ICollection<PlayerRating> PlayerRatings { get; set; }
        public virtual ICollection<VotesHistory> VotesHistories { get; set; }
    }
}
