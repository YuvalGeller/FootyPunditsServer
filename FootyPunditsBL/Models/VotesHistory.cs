using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FootyPunditsBL.Models
{
    [Table("VotesHistory")]
    public partial class VotesHistory
    {
        [Key]
        [Column("VoteID")]
        public int VoteId { get; set; }
        [Column("AccountIDFKEY")]
        public int AccountIdfkey { get; set; }
        [Column(TypeName = "date")]
        public DateTime VotedDate { get; set; }
        public int VoteType { get; set; }
        [Column("MessageID")]
        public int MessageId { get; set; }
        public bool IsUpvote { get; set; }

        [ForeignKey(nameof(AccountIdfkey))]
        [InverseProperty(nameof(UserAccount.VotesHistories))]
        public virtual UserAccount AccountIdfkeyNavigation { get; set; }
        [ForeignKey(nameof(MessageId))]
        [InverseProperty(nameof(AccMessage.VotesHistories))]
        public virtual AccMessage Message { get; set; }
    }
}
