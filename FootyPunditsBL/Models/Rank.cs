using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FootyPunditsBL.Models
{
    public partial class Rank
    {
        public Rank()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        [Key]
        [Column("RankID")]
        public int RankId { get; set; }
        public int MinUpvotes { get; set; }
        [Required]
        [StringLength(255)]
        public string RankName { get; set; }
        [Required]
        [StringLength(255)]
        public string RankLogo { get; set; }

        [InverseProperty(nameof(UserAccount.Rank))]
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
