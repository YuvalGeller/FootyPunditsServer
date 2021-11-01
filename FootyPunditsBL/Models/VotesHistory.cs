using System;
using System.Collections.Generic;

#nullable disable

namespace FootyPunditsBL.Models
{
    public partial class VotesHistory
    {
        public int VoteId { get; set; }
        public int AccountIdfkey { get; set; }
        public DateTime VotedDate { get; set; }
        public int VoteType { get; set; }
        public int MessageId { get; set; }
        public bool IsUpvote { get; set; }

        public virtual UserAccount AccountIdfkeyNavigation { get; set; }
        public virtual AccMessage Message { get; set; }
    }
}
