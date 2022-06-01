using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FootyPunditsBL.Models
{
    [Table("PlayerRating")]
    public partial class PlayerRating
    {
        [Key]
        [Column("PlayerID")]
        public int PlayerId { get; set; }
        [Column("GameID")]
        public int GameId { get; set; }
        [Column("PlayerRating")]
        public int PlayerRating1 { get; set; }
        [Column("AccountID")]
        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(UserAccount.PlayerRatings))]
        public virtual UserAccount Account { get; set; }
    }
}
