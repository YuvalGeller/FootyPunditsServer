using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FootyPunditsBL.Models
{
    [Table("AccMessage")]
    public partial class AccMessage
    {
        public AccMessage()
        {
            VotesHistories = new HashSet<VotesHistory>();
        }

        [Key]
        [Column("MessageID")]
        public int MessageId { get; set; }
        [Column("AccountID")]
        public int AccountId { get; set; }
        [Required]
        [StringLength(255)]
        public string Content { get; set; }
        [Column(TypeName = "date")]
        public DateTime SentDate { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        [Column("ChatGameID")]
        public int ChatGameId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(UserAccount.AccMessages))]
        public virtual UserAccount Account { get; set; }
        [InverseProperty(nameof(VotesHistory.Message))]
        public virtual ICollection<VotesHistory> VotesHistories { get; set; }
    }
}
