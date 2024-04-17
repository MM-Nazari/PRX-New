using Microsoft.EntityFrameworkCore;
using PRX.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PRX.Data
{
    public class PRXDbContext : DbContext
    {
        public PRXDbContext(DbContextOptions<PRXDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<HaghighiUserProfile> HaghighiUserProfiles { get; set; }
        public DbSet<HaghighiUserRelationships> HaghighiUserRelationships { get; set; }
        public DbSet<HaghighiUserFinancialProfile> HaghighiUserFinancialProfiles { get; set; }
        public DbSet<HaghighiUserFinancialChanges> HaghighiUserFinancialChanges { get; set; }
        public DbSet<HaghighiUserEducationStatus> HaghighiUserEducationStatuses { get; set; }
        public DbSet<HaghighiUserEmploymentHistory> HaghighiUserEmploymentHistories { get; set; }
        public DbSet<HaghighiUserFuturePlans> HaghighiUserFuturePlans { get; set; }
        public DbSet<HaghighiUserInvestmentExperience> HaghighiUserInvestmentExperiences { get; set; }
        public DbSet<HaghighiUserAssetType> HaghighiUserAssetTypes { get; set; }
        public DbSet<HaghighiUserUserAsset> HaghighiUserUserAssets { get; set; }
        public DbSet<HaghighiUserDebt> HaghighiUserDebts { get; set; }
        public DbSet<HaghighiUserWithdrawal> HaghighiUserWithdrawals { get; set; }
        public DbSet<HaghighiUserDeposit> HaghighiUserDeposits { get; set; }
        public DbSet<HaghighiUserInvestment> HaghighiUserInvestments { get; set; }
        public DbSet<HaghighiUserMoreInformation> HaghighiUserMoreInformation { get; set; }
        public DbSet<HaghighiUserQuestion> HaghighiUserQuestions { get; set; }
        public DbSet<HaghighiUserAnswer> HaghighiUserAnswers { get; set; }
        public DbSet<HaghighiUserAnswerOption> HaghighiUserAnswerOptions { get; set; }
        public DbSet<HaghighiUserTestScore> HaghighiUserTestScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                // Add any additional configurations for User entity
            });

            modelBuilder.Entity<HaghighiUserProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne(u => u.HaghighiUserProfile)
                    .HasForeignKey<HaghighiUserProfile>(e => e.UserId);
                // Define other configurations for the HaghighiUserProfile entity here
            });

            modelBuilder.Entity<HaghighiUserRelationships>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithMany(u => u.HaghighiUserRelationships)
                    .HasForeignKey(e => e.UserId);
                // Define other configurations for the HaghighiUserRelationships entity here
            });


            modelBuilder.Entity<HaghighiUserFinancialProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithOne()
                    .HasForeignKey<HaghighiUserFinancialProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserFinancialChanges>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserEducationStatus>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserEmploymentHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserFuturePlans>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserInvestmentExperience>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserAssetType>(entity =>
            {
                entity.HasKey(e => e.Id);
                // No foreign key relationship defined for HaghighiUserAssetType
            });

            modelBuilder.Entity<HaghighiUserUserAsset>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<HaghighiUserAssetType>()
                    .WithMany()
                    .HasForeignKey(e => e.AssetTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserDebt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserWithdrawal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserDeposit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserInvestment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserMoreInformation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserQuestion>(entity =>
            {
                entity.HasKey(e => e.Id);
                // No foreign key relationship defined for HaghighiUserQuestion
            });

            modelBuilder.Entity<HaghighiUserAnswer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<HaghighiUserQuestion>()
                    .WithMany()
                    .HasForeignKey(e => e.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserAnswerOption>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserQuestion>()
                    .WithMany()
                    .HasForeignKey(e => e.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HaghighiUserTestScore>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<HaghighiUserProfile>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }

}
