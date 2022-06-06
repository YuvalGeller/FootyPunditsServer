using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FootyPunditsBL.Models
{
    public partial class FootyPunditsDBContext : DbContext
    {
        public FootyPunditsDBContext()
        {
        }

        public FootyPunditsDBContext(DbContextOptions<FootyPunditsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccMessage> AccMessages { get; set; }
        public virtual DbSet<PlayerRating> PlayerRatings { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<VotesHistory> VotesHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server = localhost\\SQLEXPRESS; Database=FootyPunditsDB; Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<AccMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("accmessage_messageid_primary");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccMessages)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accmessage_accountid_foreign");
            });

            modelBuilder.Entity<PlayerRating>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("playerrating_playerid_primary");

                entity.Property(e => e.PlayerId).ValueGeneratedNever();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PlayerRatings)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("playerrating_accountidfkey_foreign");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.Property(e => e.RankId).ValueGeneratedNever();
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("account_accountid_primary");

                entity.Property(e => e.ProfilePicture).HasDefaultValueSql("('default_pfp.jpg')");

                entity.Property(e => e.SignUpDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.RankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("useraccount_rankid_foreign");
            });

            modelBuilder.Entity<VotesHistory>(entity =>
            {
                entity.HasKey(e => e.VoteId)
                    .HasName("voteshistory_voteid_primary");

                entity.HasOne(d => d.AccountIdfkeyNavigation)
                    .WithMany(p => p.VotesHistories)
                    .HasForeignKey(d => d.AccountIdfkey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("voteshistory_accountidfkey_foreign");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.VotesHistories)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("voteshistory_messageid_foreign");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
