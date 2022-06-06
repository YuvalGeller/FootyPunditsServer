using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FootyPunditsBL.Models
{
    [Table("UserAccount")]
    [Index(nameof(Email), Name = "account_email_unique", IsUnique = true)]
    [Index(nameof(IsAdmin), Name = "account_isadmin_index")]
    [Index(nameof(Username), Name = "account_username_unique", IsUnique = true)]
    public partial class UserAccount
    {
        public UserAccount()
        {
            AccMessages = new HashSet<AccMessage>();
            PlayerRatings = new HashSet<PlayerRating>();
            VotesHistories = new HashSet<VotesHistory>();
        }

        [Key]
        [Column("AccountID")]
        public int AccountId { get; set; }
        [Required]
        [StringLength(255)]
        public string AccName { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [Column("UPass")]
        [StringLength(255)]
        public string Upass { get; set; }
        [Required]
        [StringLength(255)]
        public string ProfilePicture { get; set; }
        public bool IsAdmin { get; set; }
        public int FavoriteTeam { get; set; }
        [Column(TypeName = "date")]
        public DateTime SignUpDate { get; set; }
        [Column("RankID")]
        public int RankId { get; set; }

        [ForeignKey(nameof(RankId))]
        [InverseProperty("UserAccounts")]
        public virtual Rank Rank { get; set; }
        [InverseProperty(nameof(AccMessage.Account))]
        public virtual ICollection<AccMessage> AccMessages { get; set; }
        [InverseProperty(nameof(PlayerRating.Account))]
        public virtual ICollection<PlayerRating> PlayerRatings { get; set; }
        [InverseProperty(nameof(VotesHistory.AccountIdfkeyNavigation))]
        public virtual ICollection<VotesHistory> VotesHistories { get; set; }
    }
}
