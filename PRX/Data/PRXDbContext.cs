using Microsoft.EntityFrameworkCore;
using PRX.Models.Haghighi;
using PRX.Models.Hoghooghi;
using PRX.Models.Hoghooghis.Hoghooghi;
using PRX.Models.Quiz;
using PRX.Models.User;
using System.Collections.Generic;
using System.Reflection.Emit;
using PRX.Models.Ticket;

namespace PRX.Data
{
    public class PRXDbContext : DbContext
    {
        public PRXDbContext(DbContextOptions<PRXDbContext> options) : base(options)
        {
        }

        //
        // Sets
        //


        // Admin 

        //public DbSet<Admin> Admins { get; set; }


        // User set

        public DbSet<User> Users { get; set; }

        public DbSet<UserFinancialChanges> UserFinancialChanges { get; set; }
        public DbSet<UserFuturePlans> UserFuturePlans { get; set; }
        public DbSet<UserInvestmentExperience> UserInvestmentExperiences { get; set; }
        public DbSet<UserAssetType> UserAssetTypes { get; set; }
        public DbSet<UserAsset> UserAssets { get; set; }
        public DbSet<UserDebt> UserDebts { get; set; }
        public DbSet<UserWithdrawal> UserWithdrawals { get; set; }
        public DbSet<UserDeposit> UserDeposits { get; set; }
        public DbSet<UserInvestment> UserInvestments { get; set; }
        public DbSet<UserMoreInformation> UserMoreInformations { get; set; }
        public DbSet<UserState> UserStates { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<UserDocument> UserDocuments { get; set; }
        public DbSet<UserReference> UserReferences { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<DataChangeLog> DataChangeLogs { get; set; }
        public DbSet<UserBankInfo> UserBankInfos { get; set; }



        // Haghighi Set


        public DbSet<HaghighiUserProfile> HaghighiUserProfiles { get; set; }
        public DbSet<HaghighiUserRelationships> HaghighiUserRelationships { get; set; }
        public DbSet<HaghighiUserFinancialProfile> HaghighiUserFinancialProfiles { get; set; }
        public DbSet<HaghighiUserEducationStatus> HaghighiUserEducationStatuses { get; set; }
        public DbSet<HaghighiUserEmploymentHistory> HaghighiUserEmploymentHistories { get; set; }
        


        // Hoghooghi Set

        public DbSet<HoghooghiUser> HoghooghiUsers { get; set; }
        public DbSet<HoghooghiUserBoardOfDirectors> HoghooghiUserBoardOfDirectors { get; set; }
        public DbSet<HoghooghiUserCompaniesWithMajorInvestors> HoghooghiUserCompaniesWithMajorInvestors { get; set; }
        public DbSet<HoghooghiUserInvestmentDepartmentStaff> HoghooghiUserInvestmentDepartmentStaff { get; set; }
        public DbSet<HoghooghiUserAssetIncomeStatusTwoYearsAgo> HoghooghiUsersAssets { get; set; }



        // Quiz Set


        public DbSet<UserQuestion> UserQuestions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserAnswerOption> UserAnswerOptions { get; set; }
        public DbSet<UserTestScore> UserTestScores { get; set; }


        // Log

        public DbSet<UserLoginLog> UserLoginLog { get; set; }


        // Ticket

        public DbSet<PRX.Models.Ticket.Ticket> Tickets { get; set; } 
        public DbSet<PRX.Models.Ticket.Message> Messages { get; set; } 








        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //
            // Primary Key & Auto Increament
            //

            // User

            modelBuilder.Entity<User>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();


            modelBuilder.Entity<UserState>()
             .Property(e => e.Id)
             .ValueGeneratedOnAdd()
             .IsRequired();

            modelBuilder.Entity<UserType>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserAssetType>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserDebt>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserDeposit>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserFinancialChanges>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserFuturePlans>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserInvestment>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserInvestmentExperience>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserMoreInformation>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserAsset>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserWithdrawal>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserDocument>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserReference>()
           .Property(e => e.Id)
           .ValueGeneratedOnAdd()
           .IsRequired();

            modelBuilder.Entity<Request>()
           .Property(e => e.Id)
           .ValueGeneratedOnAdd()
           .IsRequired();

            modelBuilder.Entity<DataChangeLog>()
           .Property(e => e.Id)
           .ValueGeneratedOnAdd()
           .IsRequired();

            modelBuilder.Entity<UserBankInfo>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();


            // Haghighi

            modelBuilder.Entity<HaghighiUserEducationStatus>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<HaghighiUserEmploymentHistory>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<HaghighiUserFinancialProfile>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<HaghighiUserProfile>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<HaghighiUserRelationships>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

    


            // Hoghooghi

            modelBuilder.Entity<HoghooghiUser>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<HoghooghiUserAssetIncomeStatusTwoYearsAgo>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<HoghooghiUserBoardOfDirectors>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<HoghooghiUserCompaniesWithMajorInvestors>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<HoghooghiUserInvestmentDepartmentStaff>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();



            // Quiz

            modelBuilder.Entity<UserAnswer>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserAnswerOption>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            modelBuilder.Entity<UserQuestion>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserTestScore>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

            // Log

            modelBuilder.Entity<UserLoginLog>()
             .Property(e => e.Id)
             .ValueGeneratedOnAdd()
             .IsRequired();


            // Ticket

            modelBuilder.Entity<PRX.Models.Ticket.Ticket>()
             .Property(e => e.Id)
             .ValueGeneratedOnAdd()
             .IsRequired();

            modelBuilder.Entity<PRX.Models.Ticket.Message>()
             .Property(e => e.Id)
             .ValueGeneratedOnAdd()
             .IsRequired();




            modelBuilder.Entity<Message>()
                .HasOne(m => m.UserSender)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            //
            // Relations
            //


            // User

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });


            modelBuilder.Entity<Request>()
               .HasOne(u => u.UserFinancialChanges)
               .WithOne(p => p.Request)
               .HasForeignKey<UserFinancialChanges>(p => p.RequestId);

            modelBuilder.Entity<Request>()
               .HasMany(u => u.EmploymentHistories)
               .WithOne(r => r.Request)
               .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasOne(u => u.UserFuturePlans)
                .WithOne(p => p.Request)
                .HasForeignKey<UserFuturePlans>(p => p.RequestId);

            modelBuilder.Entity<Request>()
                .HasMany(u => u.UserInvestmentExperiences)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasMany(u => u.UserAssets)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasMany(u => u.UserWithdrawals)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasMany(u => u.UserDeposits)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasMany(u => u.UserDebts)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasOne(u => u.UserInvestment)
                .WithOne(p => p.Request)
                .HasForeignKey<UserInvestment>(p => p.RequestId);

            modelBuilder.Entity<Request>()
                .HasOne(u => u.UserMoreInformations)
                .WithOne(p => p.Request)
                .HasForeignKey<UserMoreInformation>(p => p.RequestId);

            //modelBuilder.Entity<UserAssetType>()
            //    .HasMany(u => u.UserAssets)
            //    .WithOne(r => r.UserAssetType)
            //    .HasForeignKey(r => r.AssetTypeId);


            modelBuilder.Entity<User>()
                .HasMany(u => u.UserTypes)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserState)
                .WithOne(p => p.User)
                .HasForeignKey<UserState>(p => p.UserId);

            
            modelBuilder.Entity<Request>()
                .HasMany(u => u.UserDocs)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<User>()
               .HasOne(u => u.UserReference)
               .WithOne(p => p.User)
               .HasForeignKey<UserReference>(p => p.UserId);

            modelBuilder.Entity<User>()
             .HasMany(u => u.DataChangeLogs)
             .WithOne(r => r.User)
             .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Request>()
            .HasOne(u => u.UserBankInfos)
            .WithOne(p => p.Request)
            .HasForeignKey<UserBankInfo>(p => p.RequestId);



            // Haghighi


            // Define one-to-one relationship between User and HaghighiUserProfile
            modelBuilder.Entity<Request>()
                .HasOne(u => u.HaghighiUserProfile)
                .WithOne(p => p.Request)
                .HasForeignKey<HaghighiUserProfile>(p => p.RequestId);

            // Define one-to-many relationship between User and HaghighiUserRelationships
            modelBuilder.Entity<Request>()
                .HasMany(u => u.HaghighiUserRelationships)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
               .HasOne(u => u.HaghighiUserFinancialProfiles)
               .WithOne(p => p.Request)
               .HasForeignKey<HaghighiUserFinancialProfile>(p => p.RequestId);

            modelBuilder.Entity<Request>()
               .HasOne(u => u.HaghighiUserEducationStatus)
               .WithOne(p => p.Request)
               .HasForeignKey<HaghighiUserEducationStatus>(p => p.RequestId);





            // Hoghooghi

            modelBuilder.Entity<Request>()
               .HasOne(u => u.HoghooghiUser)
               .WithOne(p => p.Request)
               .HasForeignKey<HoghooghiUser>(p => p.RequestId);

            modelBuilder.Entity<Request>()
                .HasMany(u => u.HoghooghiUserBoardOfDirectors)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasMany(u => u.HoghooghiUserCompaniesWithMajorInvestors)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasMany(u => u.HoghooghiUserInvestmentDepartmentStaff)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasMany(u => u.HoghooghiUserAssetIncomeStatusTwoYearsAgos)
                .WithOne(r => r.Request)
                .HasForeignKey(r => r.RequestId);


            // Quiz

            modelBuilder.Entity<Request>()
               .HasMany(u => u.UserAnswer)
               .WithOne(r => r.Request)
               .HasForeignKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasOne(u => u.UserTestScore)
                .WithOne(p => p.Request)
                .HasForeignKey<UserTestScore>(p => p.RequestId);

            modelBuilder.Entity<UserQuestion>()
                .HasMany(u => u.AnswerOptions)
                .WithOne(r => r.UserQuestion)
                .HasForeignKey(r => r.QuestionId);

            modelBuilder.Entity<UserAnswerOption>()
                .HasOne(u => u.UserAnswer)
                .WithOne(p => p.answerOption)
                .HasForeignKey<UserAnswer>(p => p.AnswerOptionId);


            // Assets

            modelBuilder.Entity<UserAssetType>()
                .HasMany(u => u.UserAssets)
                .WithOne(r => r.UserAssetType)
                .HasForeignKey(r => r.AssetTypeId);


            // Log

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserLoginLogs)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);


            // Ticket


            modelBuilder.Entity<User>()
                .HasMany(u => u.Tickets)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<PRX.Models.Ticket.Ticket>()
                .HasMany(u => u.Messages)
                .WithOne(r => r.Ticket)
                .HasForeignKey(r => r.TicketId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(r => r.UserSender)
                .HasForeignKey(r => r.SenderId);

        }
    }

}
