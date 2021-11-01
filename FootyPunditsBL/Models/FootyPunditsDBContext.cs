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
                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=FootyPunditsDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<AccMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("accmessage_messageid_primary");

                entity.ToTable("AccMessage");

                entity.Property(e => e.MessageId)
                    .ValueGeneratedNever()
                    .HasColumnName("MessageID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.ChatGameId).HasColumnName("ChatGameID");

                entity.Property(e => e.ChatLeagueId).HasColumnName("ChatLeagueID");

                entity.Property(e => e.ChatPlayerId).HasColumnName("ChatPlayerID");

                entity.Property(e => e.ChatTeamId).HasColumnName("ChatTeamID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SentDate).HasColumnType("date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

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

                entity.ToTable("PlayerRating");

                entity.Property(e => e.PlayerId)
                    .ValueGeneratedNever()
                    .HasColumnName("PlayerID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.PlayerRating1).HasColumnName("PlayerRating");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PlayerRatings)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("playerrating_accountidfkey_foreign");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.Property(e => e.RankId)
                    .ValueGeneratedNever()
                    .HasColumnName("RankID");

                entity.Property(e => e.RankLogo)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RankName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("account_accountid_primary");

                entity.ToTable("UserAccount");

                entity.HasIndex(e => e.Email, "account_email_unique")
                    .IsUnique();

                entity.HasIndex(e => e.IsAdmin, "account_isadmin_index");

                entity.HasIndex(e => e.Username, "account_username_unique")
                    .IsUnique();

                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccountID");

                entity.Property(e => e.AccName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ProfilePicture)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RankId).HasColumnName("RankID");

                entity.Property(e => e.SignUpDate).HasColumnType("date");

                entity.Property(e => e.Upass)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("UPass");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255);

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

                entity.ToTable("VotesHistory");

                entity.Property(e => e.VoteId)
                    .ValueGeneratedNever()
                    .HasColumnName("VoteID");

                entity.Property(e => e.AccountIdfkey).HasColumnName("AccountIDFKEY");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.VotedDate).HasColumnType("date");

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
