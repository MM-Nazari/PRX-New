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

            //modelBuilder.Entity<Message>()
            //    .HasOne(m => m.AdminSender)
            //    .WithMany()
            //    .HasForeignKey(m => m.AdminId)
            //    .OnDelete(DeleteBehavior.Restrict);



            //
            // Unique Fields
            //

            // Admin

            //modelBuilder.Entity<Admin>()
            //    .HasIndex(p => new { p.Username })
            //    .IsUnique();


            //// User

            //modelBuilder.Entity<User>()
            //.HasIndex(p => new { p.PhoneNumber })
            //.IsUnique();


            //// Haghighi

            //modelBuilder.Entity<HaghighiUserProfile>()
            //.HasIndex(p => new { p.NationalNumber, p.Email, p.BirthCertificateNumber })
            //.IsUnique();


            // Hoghooghi




            // Quiz




            //modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);

            //
            // Seed Data
            //

            // User

            //modelBuilder.Entity<UserAssetType>().HasData(
            //    new UserAssetType { Id = 1, Name = "ساختمان و ملک" },
            //    new UserAssetType { Id = 2, Name = "خودرو" },
            //    new UserAssetType { Id = 3, Name = "طلا و ارز" },
            //    new UserAssetType { Id = 4, Name = "سهام" },
            //    new UserAssetType { Id = 5, Name = " اوراق مشارکت دولتی و شرکتی" },
            //    new UserAssetType { Id = 6, Name = "وجه نقد/ مطالبات از سایز افراد/ حساب پس انداز و سپرده بانکی" },
            //    new UserAssetType { Id = 7, Name = "سایر دارایی ها" }
            //);


            // Haghighi



            // Hoghooghi




            // Quiz

            // Q1 = Hadaf Sramaye Gozari Haghighi
            // Q2 - Q20 = Haghighi Questions
            // Q21 = Hadaf Sarmaye Gozari Hoghooghi
            // Q22 - Q33 = Hoghooghi Questions

            // modelBuilder.Entity<UserQuestion>().HasData(
            //     new UserQuestion { Id = 1, Text = "باتوجه به اینکه تصمیم به ایجاد سبد اوراق‌بهادار در اوراق‌بهادار گرفته‌اید، هدف خود را از این سرمایه‌گذاری بیان کنید", IsDeleted = false, Type = "حقیقی", QuestionNumber = 0 },
            //     new UserQuestion { Id = 2, Text = "معمولاً وضعيت پس انداز شما به چه صورت است؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 1 },
            //     new UserQuestion { Id = 3, Text = "میزان آشنایی شما با امور سرمایه‌گذاری در اوراق بهادار چقدر است؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 2 },
            //     new UserQuestion { Id = 4, Text = "اگر ارزش سرمایه‌گذاری شما در یک دورۀ کوتاه مدت – مثلاً دو هفته- 6 درصد کاهش یابد، چه عکس‌العملی خواهید داشت؟ (اگر در گذشته چنین چیزی را تجربه کرده‌اید، همان واکنشی را که انجام دادید انتخاب نمایید)", IsDeleted = false, Type = "حقیقی", QuestionNumber = 3 },
            //     new UserQuestion { Id = 5, Text = "اگر ارزش سرمایه‌گذاری شما در یک دورۀ کوتاه مدت – مثلاً سه ماه- 40 درصد کاهش یابد، چه عکس‌العملی خواهید داشت؟ (اگر در گذشته چنین چیزی را تجربه کرده‌اید، همان واکنشی را که انجام دادید انتخاب نمایید)", IsDeleted = false, Type = "حقیقی", QuestionNumber = 4 },
            //     new UserQuestion { Id = 6, Text = "اگر مایل به سرمایه‌گذاری در سهام شرکت ها هستید، کدام یک از موارد زیر را ترجیح می‌دهید؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 5 },
            //     new UserQuestion { Id = 7, Text = "اگر ناچار باشید بین دو شغل، یکی با امنیت شغلی بالا و حقوق معقول و دیگری با امنیت کمتر ولی حقوق بالا، یکی را انتخاب نمایید، کدام‌یک را برمی‌گزینید؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 6 },
            //     new UserQuestion { Id = 8, Text = "اگر یک ثروت قابل توجه به طور غیر منتظره، مثلاً یک ارث قابل توجه به دست آورید، با آن چه خواهید کرد؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 7 },
            //     new UserQuestion { Id = 9, Text = "دیدگاه شما هنگام انجام یک سرمایه‌گذاری چیست؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 8 },
            //     new UserQuestion { Id = 10, Text = "فکر می‌کنید تا چه حد برای تامین هزینه‌های معمول زندگی تان به درآمد این سرمایه‌گذاری وابسته باشید؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 9 },
            //     new UserQuestion { Id = 11, Text = "منبع عمدۀ کسب اطلاعات اقتصادی شما کدام است؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 10 },
            //     new UserQuestion { Id = 12, Text = "آیا تاکنون برای انجام سرمایه‌گذاری، وجهی از دیگران قرض گرفته‌اید؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 11 },
            //     new UserQuestion { Id = 13, Text = "درصورتی‌که قصد دارید به یک مسافرت تفریحی بروید، کدام گزینه به رفتار شما قبل از این سفر نزدیک تر است؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 12 },
            //     new UserQuestion { Id = 14, Text = "فرض کنید مدت 6 ماه است سرمایه‌گذاری انجام داده اید و پس از گذشت این مدت سرمایه‌گذاری شما با زیان مواجه شده است، واکنش شما چیست؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 13 },
            //     new UserQuestion { Id = 15, Text = "اگر پس اندازی داشته باشید که آن را طی چند سال به دست آورده باشید، چگونه آن را سرمایه‌گذاری می‌کنید؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 14 },
            //     new UserQuestion { Id = 16, Text = "درصورتی‌که شرکتهای بیمه خدمات بیمه مناسب ارائه کنند، در کدام یک از موارد زیر اقدام به تهیه بیمه نامه می‌کنید؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 15 },
            //     new UserQuestion { Id = 17, Text = "اگر امکان سرمایه‌گذاری مبلغ 100 میلیون تومان داشته باشید، کدام فرصت سرمایه‌گذاری را ترجیح می‌دهید؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 16 },
            //     new UserQuestion { Id = 18, Text = "به نظرتان اگر از دوستانتان دربارۀ شخصیت شما سوال شود، توصیف آنها در مورد شما به کدام مورد زیر نزدیک‌تر است:", IsDeleted = false, Type = "حقیقی", QuestionNumber = 17 },
            //     new UserQuestion { Id = 19, Text = "فرض کنید برای یک سفر جالب برنامه‌ریزی کرده‌اید که همیشه امکان آن برایتان میسر نیست، قبل از سفر به طور ناگهانی کار خود را از دست می‌دهید، واکنش شما در خصوص این مسافرت به چه صورت خواهد بود؟", IsDeleted = false, Type = "حقیقی", QuestionNumber = 18 },
            //     new UserQuestion { Id = 20, Text = "در شکل زیر نمودار تغییرات ارزش 4 پورتفوی فرضی در مدت 5 سال نشان داده شده است: ارزش پورتفوی A در این مدت 2 برابر شده است، در بعضی از سالها سود بالا و در بعضی سال ها نیز زیان سنگینی داشته است. پورتفوی D رشد نسبتاً کمتری داشته است و سالانه بازدهی منطقی داشته است. پورتفوی B و C از نظر رشد ارزش پورتفوی و هم چنین نوسانات سالانه بین پورتفوهای A و D قرار می‌گیرند. با توجه به موقعیت خودتان و دلایلتان برای سرمایه‌گذاری در آینده کدام پورتفوی را برای سرمایه‌گذاری مناسب می‌دانید؟ (محور عمودی ارزش روز پورتفوی و محور افقی سال‌های متناظر می‌باشد)", IsDeleted = false, Type = "حقیقی", QuestionNumber = 19 },
            //     new UserQuestion { Id = 21, Text = "باتوجه به اینکه تصمیم به ایجاد سرمایه گذاری در اوراق‌بهادار گرفته‌اید، هدف خود را از این سرمایه‌گذاری بیان کنید:", IsDeleted = false, Type = "حقوقی", QuestionNumber = 0 },
            //     new UserQuestion { Id = 22, Text = "معمولاً وضعيت پس انداز (وجوه نقد و شبه نقد)  شرکت شما به چه صورت است؟", IsDeleted = false, Type = "حقوقی", QuestionNumber = 1 },
            //     new UserQuestion { Id = 23, Text = "میزان آشنایی شرکت شما با امور سرمایه‌گذاری در اوراق بهادار چقدر است؟", IsDeleted = false, Type = "حقوقی", QuestionNumber = 2 },
            //     new UserQuestion { Id = 24, Text = "اگر ارزش پرتفوی شرکت در یک دورۀ کوتاه مدت – مثلاً دو هفته- 6 درصد کاهش یابد، چه عکس‌العملی خواهید داشت؟ (اگر در گذشته چنین چیزی را تجربه کرده‌اید، همان واکنشی را که انجام دادید انتخاب نمایید)", IsDeleted = false, Type = "حقوقی", QuestionNumber = 3 },
            //     new UserQuestion { Id = 25, Text = "اگر ارزش سرمایه‌گذاری شرکت در یک دورۀ کوتاه مدت – مثلاً سه ماه- 40 درصد کاهش یابد، چه عکس‌العملی خواهید داشت؟ (اگر در گذشته چنین چیزی را تجربه کرده‌اید، همان واکنشی را که انجام دادید انتخاب نمایید)", IsDeleted = false, Type = "حقوقی", QuestionNumber = 4 },
            //     new UserQuestion { Id = 26, Text = "اگر شرکت مایل به سرمایه‌گذاری در سهام شرکت ها باشد، سرمایه¬گذاری در کدام یک از موارد زیر را در اولویت قرار می¬دهد؟", IsDeleted = false, Type = "حقوقی", QuestionNumber = 5 },
            //     new UserQuestion { Id = 27, Text = "دیدگاه شرکت در انجام سرمایه¬گذاری¬ها به کدام یک از دیدگاه¬های زیر نزدیک¬تر می باشد؟", IsDeleted = false, Type = "حقوقی", QuestionNumber = 6 },
            //     new UserQuestion { Id = 28, Text = "منبع عمدۀ کسب اطلاعات اقتصادی شرکت کدام است؟", IsDeleted = false, Type = "حقوقی", QuestionNumber = 7 },
            //     new UserQuestion { Id = 29, Text = "آیا شرکت تاکنون برای انجام سرمایه‌گذاری، وجهی را به عنوان وام دریافت نموده است؟", IsDeleted = false, Type = "حقوقی", QuestionNumber = 8 },
            //     new UserQuestion { Id = 30, Text = "شرکت  تا چه حد برای تامین هزینه‌های خود به درآمد حاصل از سرمایه¬گذاری در اوراق بهادار وابسته می باشد؟", IsDeleted = false, Type = "حقوقی", QuestionNumber = 9 },
            //     new UserQuestion { Id = 31, Text = "اگر شرکت منابع مالی داشته باشد که آن را طی چند سال به دست آورده ، چگونه آن را سرمایه‌گذاری می‌کنید؟", IsDeleted = false, Type = "حقوقی", QuestionNumber = 10 },
            //     new UserQuestion { Id = 32, Text = "درصورتیکه شرکت امکان سرمایه‌گذاری به مبلغ 100 میلیون تومان داشته باشد، کدام فرصت سرمایه‌گذاری را ترجیح می‌دهد؟", IsDeleted = false, Type = "حقوقی", QuestionNumber = 11 },
            //     new UserQuestion { Id = 33, Text = "در شکل زیر نمودار تغییرات ارزش 4 پورتفوی فرضی در مدت 5 سال نشان داده شده است:ارزش پورتفوی A در این مدت 2 برابر شده است، در بعضی از سالها سود بالا و در بعضی سال ها نیز زیان سنگینی داشته است.پورتفوی D رشد نسبتاً کمتری داشته است و سالانه بازدهی منطقی داشته است.پورتفوی B و  Cاز نظر رشد ارزش پورتفوی و هم چنین نوسانات سالانه بین پورتفوهای A و D قرار می‌گیرند.", IsDeleted = false, Type = "حقوقی", QuestionNumber = 12 }
            // );

            // modelBuilder.Entity<UserQuestion>().HasData(
            //    new UserQuestion { Id = 1, Text = "باتوجه به اینکه تصمیم به ایجاد سبد اوراق‌بهادار در اوراق‌بهادار گرفته‌اید، هدف خود را از این سرمایه‌گذاری بیان کنید" },
            //    new UserQuestion { Id = 2, Text = "معمولاً وضعيت پس انداز شما به چه صورت است؟".Replace("\t", "") },
            //    new UserQuestion { Id = 3, Text = "میزان آشنایی شما با امور سرمایه‌گذاری در اوراق بهادار چقدر است؟" },
            //    new UserQuestion { Id = 4, Text = "اگر ارزش سرمایه‌گذاری شما در یک دورۀ کوتاه مدت – مثلاً دو هفته- 6 درصد کاهش یابد، چه عکس‌العملی خواهید داشت؟ (اگر در گذشته چنین چیزی را تجربه کرده‌اید، همان واکنشی را که انجام دادید انتخاب نمایید)" },
            //    new UserQuestion { Id = 5, Text = "اگر ارزش سرمایه‌گذاری شما در یک دورۀ کوتاه مدت – مثلاً سه ماه- 40 درصد کاهش یابد، چه عکس‌العملی خواهید داشت؟ (اگر در گذشته چنین چیزی را تجربه کرده‌اید، همان واکنشی را که انجام دادید انتخاب نمایید)" },
            //    new UserQuestion { Id = 6, Text = "اگر مایل به سرمایه‌گذاری در سهام شرکت ها هستید، کدام یک از موارد زیر را ترجیح می‌دهید؟" },
            //    new UserQuestion { Id = 7, Text = "اگر ناچار باشید بین دو شغل، یکی با امنیت شغلی بالا و حقوق معقول و دیگری با امنیت کمتر ولی حقوق بالا، یکی را انتخاب نمایید، کدام‌یک را برمی‌گزینید؟ " },
            //    new UserQuestion { Id = 8, Text = "اگر یک ثروت قابل توجه به طور غیر منتظره، مثلاً یک ارث قابل توجه به دست آورید، با آن چه خواهید کرد؟" },
            //    new UserQuestion { Id = 9, Text = "دیدگاه شما هنگام انجام یک سرمایه‌گذاری چیست؟" },
            //    new UserQuestion { Id = 10, Text = "فکر می‌کنید تا چه حد برای تامین هزینه‌های معمول زندگی تان به درآمد این سرمایه‌گذاری وابسته باشید؟" },
            //    new UserQuestion { Id = 11, Text = "منبع عمدۀ کسب اطلاعات اقتصادی شما کدام است؟" },
            //    new UserQuestion { Id = 12, Text = "آیا تاکنون برای انجام سرمایه‌گذاری، وجهی از دیگران قرض گرفته‌اید؟" },
            //    new UserQuestion { Id = 13, Text = "درصورتی‌که قصد دارید به یک مسافرت تفریحی بروید، کدام گزینه به رفتار شما قبل از این سفر نزدیک تر است؟" },
            //    new UserQuestion { Id = 14, Text = "فرض کنید مدت 6 ماه است سرمایه‌گذاری انجام داده اید و پس از گذشت این مدت سرمایه‌گذاری شما با زیان مواجه شده است، واکنش شما چیست؟" },
            //    new UserQuestion { Id = 15, Text = "اگر پس اندازی داشته باشید که آن را طی چند سال به دست آورده باشید، چگونه آن را سرمایه‌گذاری می‌کنید؟" },
            //    new UserQuestion { Id = 16, Text = "درصورتی‌که شرکتهای بیمه خدمات بیمه مناسب ارائه کنند، در کدام یک از موارد زیر اقدام به تهیه بیمه نامه می‌کنید؟" },
            //    new UserQuestion { Id = 17, Text = "اگر امکان سرمایه‌گذاری مبلغ 100 میلیون تومان داشته باشید، کدام فرصت سرمایه‌گذاری را ترجیح می‌دهید؟" },
            //    new UserQuestion { Id = 18, Text = "به نظرتان اگر از دوستانتان دربارۀ شخصیت شما سوال شود، توصیف آنها در مورد شما به کدام مورد زیر نزدیک‌تر است:" },
            //    new UserQuestion { Id = 19, Text = "فرض کنید برای یک سفر جالب برنامه‌ریزی کرده‌اید که همیشه امکان آن برایتان میسر نیست، قبل از سفر به طور ناگهانی کار خود را از دست می‌دهید، واکنش شما در خصوص این مسافرت به چه صورت خواهد بود؟" },
            //    new UserQuestion { Id = 20, Text = "در شکل زیر نمودار تغییرات ارزش 4 پورتفوی فرضی در مدت 5 سال نشان داده شده استارزش پورتفوی A در این مدت 2 برابر شده است، در بعضی از سالها سود بالا و در بعضی سال ها نیز زیان سنگینی داشته است.پورتفوی D رشد نسبتاً کمتری داشته است و سالانه بازدهی منطقی داشته است.پورتفوی B و  Cاز نظر رشد ارزش پورتفوی و هم چنین نوسانات سالانه بین پورتفوهای A و D قرار می‌گیرند.nبا توجه به موقعیت خودتان و دلایلتان برای سرمایه‌گذاری در آینده کدام پورتفوی را برای سرمایه‌گذاری مناسب می‌دانید؟ (محور عمودی ارزش روز پورتفوی و محور افقی سال‌های متناظر می‌باشد)" },
            //    new UserQuestion { Id = 21, Text = "باتوجه به اینکه تصمیم به ایجاد سرمایه گذاری در اوراق‌بهادار گرفته‌اید، هدف خود را از این سرمایه‌گذاری بیان کنید:" },
            //    new UserQuestion { Id = 22, Text = "معمولاً وضعيت پس انداز (وجوه نقد و شبه نقد)  شرکت شما به چه صورت است؟" },
            //    new UserQuestion { Id = 23, Text = "میزان آشنایی شرکت شما با امور سرمایه‌گذاری در اوراق بهادار چقدر است؟" },
            //    new UserQuestion { Id = 24, Text = "اگر ارزش پرتفوی شرکت در یک دورۀ کوتاه مدت – مثلاً دو هفته- 6 درصد کاهش یابد، چه عکس‌العملی خواهید داشت؟ (اگر در گذشته چنین چیزی را تجربه کرده‌اید، همان واکنشی را که انجام دادید انتخاب نمایید)" },
            //    new UserQuestion { Id = 25, Text = "اگر ارزش سرمایه‌گذاری شرکت در یک دورۀ کوتاه مدت – مثلاً سه ماه- 40 درصد کاهش یابد، چه عکس‌العملی خواهید داشت؟ (اگر در گذشته چنین چیزی را تجربه کرده‌اید، همان واکنشی را که انجام دادید انتخاب نمایید)" },
            //    new UserQuestion { Id = 26, Text = "اگر شرکت مایل به سرمایه‌گذاری در سهام شرکت ها باشد، سرمایه¬گذاری در کدام یک از موارد زیر را در اولویت قرار می¬دهد؟" },
            //    new UserQuestion { Id = 27, Text = "دیدگاه شرکت در انجام سرمایه¬گذاری¬ها به کدام یک از دیدگاه¬های زیر نزدیک¬تر می باشد؟" },
            //    new UserQuestion { Id = 28, Text = "منبع عمدۀ کسب اطلاعات اقتصادی شرکت کدام است؟" },
            //    new UserQuestion { Id = 29, Text = "آیا شرکت تاکنون برای انجام سرمایه‌گذاری، وجهی را به عنوان وام دریافت نموده است؟" },
            //    new UserQuestion { Id = 30, Text = "شرکت  تا چه حد برای تامین هزینه‌های خود به درآمد حاصل از سرمایه¬گذاری در اوراق بهادار وابسته می باشد؟" },
            //    new UserQuestion { Id = 31, Text = "اگر شرکت منابع مالی داشته باشد که آن را طی چند سال به دست آورده ، چگونه آن را سرمایه‌گذاری می‌کنید؟" },
            //    new UserQuestion { Id = 32, Text = "درصورتیکه شرکت امکان سرمایه‌گذاری به مبلغ 100 میلیون تومان داشته باشد، کدام فرصت سرمایه‌گذاری را ترجیح می‌دهد؟" },
            //    new UserQuestion { Id = 33, Text = "در شکل زیر نمودار تغییرات ارزش 4 پورتفوی فرضی در مدت 5 سال نشان داده شده است:ارزش پورتفوی A در این مدت 2 برابر شده است، در بعضی از سالها سود بالا و در بعضی سال ها نیز زیان سنگینی داشته است.پورتفوی D رشد نسبتاً کمتری داشته است و سالانه بازدهی منطقی داشته است.پورتفوی B و  Cاز نظر رشد ارزش پورتفوی و هم چنین نوسانات سالانه بین پورتفوهای A و D قرار می‌گیرند." }
            //);

            // modelBuilder.Entity<UserAnswerOption>().HasData(
            // //new UserAnswerOption { Id = 3, QuestionId = 1, Text = "حفظ ارزش دارایی.", Score = 0, IsDeleted = false },
            // //new UserAnswerOption { Id = 4, QuestionId = 1, Text = "استفاده از عایدات سرمایه‌گذاری جهت تأمین هزینه‌های خود و افراد تحت تکفل.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 5, QuestionId = 1, Text = "استفاده از عایدات سرمایه‌گذاری به عنوان در‌آمد ثانویه (جهت افزایش رفاه زندگی).", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 4, QuestionId = 1, Text = "تأمین وجه مورد نیاز جهت خرید مسکن", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 5, QuestionId = 1, Text = "سایر موارد، به تفصیل شرح داده شود: ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 6, QuestionId = 2, Text = "درآمدهای معمول من به میزانی نبوده است که بتوانم پس اندازی داشته باشم.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 7, QuestionId = 2, Text = "معمولاً بیش از 50 % پس انداز من به صورت نقد و شبه نقد (شامل سپردۀ بانکی و اوراق مشارکت) بوده است", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 8, QuestionId = 2, Text = "معمولاً کمتر از 50 % پس انداز من به صورت نقد و شبه نقد (شامل سپردۀ بانکی و اوراق مشارکت) بوده است. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 9, QuestionId = 2, Text = "اصولاً پس انداز خود را به صورت نقد یا شبه نقد نگهداري نمي كنم و در اولین فرصت آن را در سایر دارایی‌های مالی یا دارایی‌های فیزیکی مختلف سرمایه‌گذاری می‌کنم", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 10, QuestionId = 3, Text = "به هیچ وجه با سرمایه‌گذاری در اوراق بهادار آشنایی ندارم. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 11, QuestionId = 3, Text = "تا حدودی با سرمایه‌گذاری در اوراق بهادار آشنا هستم ولی درک کاملی از آن ندارم.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 12, QuestionId = 3, Text = " با سرمایه‌گذاری در اوراق بهادار آشنا هستم. عوامل مختلفی که بر بازده سرمایه‌گذاری مؤثر هستند را درک می‌کنم. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 13, QuestionId = 3, Text = "آشنایی زیادی با سرمایه‌گذاری در اوراق بهادار دارم. در اتخاذ تصمیمات سرمایه‌گذاری خود، از تحقیقات انجام شده و سایر اطلاعات مرتبط استفاده می‌کنم. عوامل مختلفی که بر بازده سرمایه‌گذاری مؤثر هستند را درک می‌کنم. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 14, QuestionId = 4, Text = "تمامی سرمایه‌گذاری خود را می‌فروشید. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 15, QuestionId = 4, Text = "قسمتی از سرمایه‌گذاری خود را می‌فروشید.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 16, QuestionId = 4, Text = "هیچ قسمتی از سرمایه‌گذاری خود را نمی‌فروشید", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 17, QuestionId = 4, Text = "وجوه بیشتری را به سرمایه‌گذاری اختصاص می‌دهید و می‌توانید زیان‌های کوتاه‌مدت را به انتظار رشد افزایش سرمایه‌گذاری خود در آینده، بپذیرید. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 18, QuestionId = 5, Text = "تمامی سرمایه‌گذاری خود را می‌فروشید و تمایلی به پذیرش ریسک بیشتر ندارید. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 19, QuestionId = 5, Text = " قسمتی از سرمایه‌گذاری خود را فروخته و مبلغ آن را در دارایی‌های کم‌ریسک‌تر سرمایه‌گذاری می‌کنید. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 20, QuestionId = 5, Text = "به امید بهبود شرایط، سرمایه‌گذاری خود را نمی‌فروشید. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 21, QuestionId = 5, Text = "مبلغ دیگری به سرمایه‌گذاری خود اضافه کرده و سعی می‌کنید بهای تمام شده خود را کاهش دهید. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 22, QuestionId = 6, Text = "سهام شرکت‌های شناخته‌شده و بزرگ ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 23, QuestionId = 6, Text = " سهام شرکت‌های تازه‌وارد به بورس که زیاد شناخته شده نیستند ولی ممکن است در آینده سودآوری بالایی داشته باشند.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 24, QuestionId = 7, Text = "یقیناً شغل با حقوق بالاتر و امنیت کمتر را ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 25, QuestionId = 7, Text = "احتمالاً شغل با حقوق بالاتر و امنیت کمتر را ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 26, QuestionId = 7, Text = "باید سایر جنبه‌ها را در نظر بگیرم ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 27, QuestionId = 7, Text = "یقیناً شغل دارای امنیت بیشتر و حقوق کمتر را ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 28, QuestionId = 8, Text = "درصورتی‌که فرصت مناسب ایجاد شود، شغل فعلی را رها کرده و با پول موردنظر کسب و کار جدیدی را آغاز خواهم نمود. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 29, QuestionId = 8, Text = "سعی می‌کنم ثروت به دست آمده را در دارائی‌های مختلف با ریسک متفاوت، سرمایه‌گذاری کنم.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 30, QuestionId = 8, Text = "قسمتی از آن را در بانک سرمایه‌گذاری کرده و در مورد سرمایه‌گذاری باقی‌ماندۀ آن، با افراد آگاه مشورت خواهم کرد. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 31, QuestionId = 8, Text = "اصولاً آن را در حساب سپرده سرمایه‌گذاری می‌کنم. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 32, QuestionId = 9, Text = "می‌خواهم سرمایه‌گذاری مطمئنی داشته باشم که بازدهی معقولی برای من ایجاد کند", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 33, QuestionId = 9, Text = "می‌خواهم ارزش سرمایه‌گذاری‌ام در بلندمدت رشد کند. از نظر من نوسانات بازده در کوتاه‌مدت قابل قبول هستند", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 34, QuestionId = 9, Text = "من در پی رشد ارزش سرمایه‌گذاری در بلندمدت به صورت جسورانه هستم و آمادگی پذیرش نوسانات قابل ملاحظه بازار را هم دارم. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 35, QuestionId = 10, Text = "درآمد اصلی من از فعالیت دیگری است که اصولاً کفاف هزینه‌های عادی زندگی مرا می‌دهد", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 36, QuestionId = 10, Text = "کمی به درآمد این سرمایه‌گذاری وابسته هستم، به عبارتی درآمد این سرمایه‌گذاری می‌تواند کمک خرج من باشد. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 37, QuestionId = 10, Text = "تقریبا به درآمد این سرمایه‌گذاری برای تامین هزینه‌های معمول زندگی ام وابسته ام و به عنوان یک درآمد دوم روی آن حساب می‌کنم", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 38, QuestionId = 10, Text = "به شدت به درآمد این سرمایه‌گذاری وابسته‌ام و برای تامین هزینه‌های معمول زندگی ام به این سرمایه‌گذاری متکی هستم.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 39, QuestionId = 11, Text = "رادیو و تلویزیون ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 40, QuestionId = 11, Text = "روزنامه‌های عمومی ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 41, QuestionId = 11, Text = "روزنامه‌های اقتصادی ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 42, QuestionId = 11, Text = "سایت‌های تخصصی بازار سرمایه، مجلات تخصصی و روزنامه‌های اقتصادی ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 43, QuestionId = 12, Text = "بله", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 44, QuestionId = 12, Text = "خیر", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 45, QuestionId = 13, Text = "مطمئناً چند روز قبل از سفر برنامه ریزی کامل کرده و در مورد رزرو هتل و سایر موارد اقدامات لازم را انجام می‌دهم.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 46, QuestionId = 13, Text = "معمولاً برای مسافرت برنامه ریزی می‌کنم.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 47, QuestionId = 13, Text = "معمولاً به صورت اتفاقی به سفر می‌روم و رزرو هتل و سایر موراد چندان در تصمیم من برای مسافرت نقشی ندارد", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 48, QuestionId = 14, Text = "درک می‌کنم که هر سرمایه‌گذاری ممکن است با زیان مواجه شود. ", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 49, QuestionId = 14, Text = "زیان در این بازه زمانی کوتاه مدت نگران کننده نیست.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 50, QuestionId = 14, Text = "اگر ضرری بیش از 10% رخ داده باشد، نگران خواهم شد.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 51, QuestionId = 14, Text = "هر ضرری که رخ دهد مرا نگران می‌کند", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 52, QuestionId = 15, Text = "در دارائی‌های کم ریسک مانند سپرده بانکی یا اوراق بهادار با درآمد ثابت", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 53, QuestionId = 15, Text = " در دارائی‌های با ریسک متوسط مانند واحدهای صندوق‌های سرمایه‌گذاری مشترک", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 54, QuestionId = 15, Text = "در دارائی‌های با ریسک زیاد مانند سهام شرکتها", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 55, QuestionId = 15, Text = "در دارائی‌های با ریسک خیلی زیاد مانند قراردادهای آتی و اوراق اختیار معامله", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 56, QuestionId = 16, Text = "اصولاً اعتقادی به بیمه ندارم و تا زمانی که از نظر قانونی مجبور به تهیه بیمه نامه نشوم، بیمه نامه تهیه نمی کنم.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 57, QuestionId = 16, Text = "تنها بیمه نامه‌های مربوط به خودرو را تهیه می‌کنم.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 58, QuestionId = 16, Text = "معمولاً برای کل دارائی‌های با اهمیت مانند ساختمان و خودرو بیمه نامه‌های لازم را تهیه می‌کنم.", IsDeleted = false, Score = 0 },
            // //new UserAnswerOption { Id = 59, QuestionId = 16, Text = "علاوه بر دارائیهای با ارزش بالا، برای سایر دارائی ها از جمله کامپیوتر شخصی، تلفن همراه، دوربین و . . . نیز گارانتی و بیمه نامه تهیه می‌کنم.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 60, QuestionId = 17, Text = "سرمایه‌گذاری در پروژه ای که قطعاً 20 میلیون تومان سود به همراه خواهد داشت.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 61, QuestionId = 17, Text = "سرمایه‌گذاری در پروژه ای که احتمال دارد 40 میلیون تومان سود کسب کنید یا امکان دارد هیچ سودی به دست نیاورید.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 62, QuestionId = 17, Text = "سرمایه‌گذاری در پروژه ای که احتمال دارد 80 میلیون تومان سود کسب کنید یا امکان دارد 40 میلیون تومان ضرر کنید.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 63, QuestionId = 17, Text = "سرمایه‌گذاری در پروژه ای که احتمال دارد 120 میلیون تومان سود کسب کنید یا امکان دارد 80 میلیون تومان ضرر کنید. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 64, QuestionId = 18, Text = "از نظر آنها من شخصی خوش‌بین هستم و خیلی خود را درگیر جزئیات کارها نمی کنم.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 65, QuestionId = 18, Text = "از نظر آنها من تاحدودی خوش‌بین هستم و بعد از فکر و برنامه‌ریزی لازم اقدام به انجام هرکاری می‌کنم.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 66, QuestionId = 18, Text = "از نظر آنها من شخصی محاسبه گر و محتاط هستم.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 67, QuestionId = 18, Text = "از نظر آنها من شخصی شدیداً محتاط و محاسبه‌گر هستم و برای انجام هر کاری تمام جوانب آن را می‌سنجم.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 68, QuestionId = 19, Text = " مسافرت را لغو می‌کنم.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 69, QuestionId = 19, Text = "این مسافرت را لغو و به جای آن برای یک مسافرت محدودتر برنامه‌ریزی می‌کنم.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 70, QuestionId = 19, Text = " طبق برنامه ریزی قبلی به مسافرت رفته و بعد از برگشت از سفر دنبال شغل جدید می‌گردم.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 71, QuestionId = 19, Text = "با خیال راحت به مسافرت می‌روم و حتی با فراغ خاطر بیشتری از آن لذت می‌برم، مطمئنم که بعداً شغل بهتری خواهم یافت.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 72, QuestionId = 20, Text = "PORTFOLIO A", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 73, QuestionId = 20, Text = "PORTFOLIO B", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 74, QuestionId = 20, Text = "PORTFOLIO C", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 75, QuestionId = 20, Text = "PORTFOLIO D", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 76, QuestionId = 21, Text = "حفظ ارزش دارایی.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 77, QuestionId = 21, Text = "استفاده از عایدات سرمایه‌گذاری جهت تأمین هزینه‌های شرکت", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 78, QuestionId = 21, Text = "استفاده از عایدات سرمایه‌گذاری به عنوان در‌آمد ثانویه (جهت افزایش منابع مالی).", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 79, QuestionId = 21, Text = "تأمین وجه مورد نیاز جهت خرید دارایی ثابت.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 80, QuestionId = 21, Text = "سایر موارد، به تفصیل شرح داده شود: ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 81, QuestionId = 22, Text = "وضعیت نقدینگی شرکت معمولا  به میزانی نبوده است که بتواند پس اندازی داشته باشد.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 81, QuestionId = 22, Text = "معمولاً بیش از 50 % پس انداز شرکت به صورت نقد و شبه نقد (شامل سپردۀ بانکی و اوراق مشارکت) بوده است. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 82, QuestionId = 22, Text = "معمولاً کمتر از 50 % پس انداز شرکت به صورت نقد و شبه نقد (شامل سپردۀ بانکی و اوراق مشارکت) بوده است. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 83, QuestionId = 22, Text = "شرکت اصولاً پس انداز خود را به صورت نقد یا شبه نقد نگهداري نمي كند و در اولین فرصت آن را در سایر دارایی‌های مالی یا دارایی‌های فیزیکی مختلف سرمایه‌گذاری می‌نماید.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 84, QuestionId = 23, Text = "شرکت دارای پیشینه¬ای در زمینه سرمایه گذاری در اوراق بهادار نمی¬باشد. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 85, QuestionId = 23, Text = "شرکت با مشورت افراد آگاه سرمایه گذاری¬های موردی و کوتاه مدت در اوراق بهادار داشته است.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 86, QuestionId = 23, Text = "شرکت با سرمایه‌گذاری در اوراق بهادار و عوامل مختلفی که بر بازده سرمایه‌گذاری مؤثر هستند آشنایی دارد.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 87, QuestionId = 23, Text = "شرکت دارای درک کاملی از سرمایه‌گذاری در اوراق بهادار و عوامل مختلف مؤثر بر بازده سرمایه‌گذاری می¬باشد و از تحقیقات انجام شده و سایر اطلاعات مرتبط استفاده می¬نماید.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 88, QuestionId = 24, Text = "شرکت تمام سرمایه‌گذاری¬های خود را می‌فروشد. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 89, QuestionId = 24, Text = "شرکت بخشی از سرمایه‌گذاری خود را می‌فروشد.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 90, QuestionId = 24, Text = "شرکت ترکیب پرتفوی خود را حفظ می¬نماید", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 91, QuestionId = 24, Text = "شرکت وجوه بیشتری را به سرمایه‌گذاری اختصاص می‌دهد و می‌تواند زیان‌های کوتاه‌مدت را به انتظار رشد افزایش سرمایه‌گذاری خود در آینده، بپذیرد. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 92, QuestionId = 25, Text = "شرکت تمام سرمایه‌گذاری¬های خود را می‌فروشد و تمایلی به پذیرش ریسک بیشتر نشان نمی¬دهد. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 93, QuestionId = 25, Text = "شرکت بخشی از سرمایه‌گذاری خود را فروخته و مبلغ آن را در دارایی‌های کم‌ریسک‌تر سرمایه‌گذاری می¬نماید. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 94, QuestionId = 25, Text = "شرکت به امید بهبود شرایط، ترکیب پرتفوی خود را حفظ می¬نماید. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 95, QuestionId = 25, Text = "شرکت آورده جدیدی را به پرتفوی سرمایه‌گذاری خود اضافه کرده و سعی می‌کند بهای تمام شده خود را کاهش دهد. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 96, QuestionId = 26, Text = "سهام شرکت‌های شناخته‌شده و بزرگ ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 97, QuestionId = 26, Text = "سهام شرکت‌های تازه‌وارد به بورس که زیاد شناخته شده نیستند ولی ممکن است در آینده سودآوری بالایی داشته باشند.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 98, QuestionId = 26, Text = "ترکیبی از هر دو", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 99, QuestionId = 27, Text = "دیدگاه شرکت بر اساس سرمایه‌گذاری مطمئن و بازده معقول تنظیم و متناسب با ریسک مربوطه گردیده است.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 100, QuestionId = 27, Text = "دیدگاه شرکت بر اساس رشد بلند مدت ارزش سرمایه‌گذاری¬ها تنظیم گردیده است و از نظر شرکت نوسانات بازده در کوتاه‌مدت قابل قبول می¬باشد. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 101, QuestionId = 27, Text = "دیدگاه شرکت بر پایه رشد ارزش سرمایه‌گذاری در بلندمدت به صورت جسورانه تنظیم گردیده است و آمادگی پذیرش نوسانات قابل ملاحظه بازار را نیز دارد. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 102, QuestionId = 28, Text = "رادیو و تلویزیون ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 103, QuestionId = 28, Text = "روزنامه‌های عمومی ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 104, QuestionId = 28, Text = "روزنامه‌های اقتصادی ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 105, QuestionId = 28, Text = "سایت‌های تخصصی بازار سرمایه، مجلات تخصصی و روزنامه‌های اقتصادی ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 106, QuestionId = 29, Text = "بله", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 107, QuestionId = 29, Text = "خیر", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 108, QuestionId = 30, Text = "درآمد شرکت از  فعالیت¬های عملیاتی و سایر تامین می¬گردد که هزینه¬های شرکت را پوشش می¬دهد. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 109, QuestionId = 30, Text = "شرکت اندکی به درآمد حاصل از سرمایه‌گذاری وابسته می¬باشد، به عبارتی درآمد این سرمایه‌گذاری می‌تواند بخشی از منابع مالی مورد نیاز شرکت را تامین کند. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 110, QuestionId = 30, Text = "شرکت تقریبا به درآمد این سرمایه‌گذاری برای تامین هزینه‌های معمول خود وابسته می¬باشد و روی درآمد حاصل از این سرمایه گذاری¬ها حساب می‌کند. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 111, QuestionId = 30, Text = " شرکت به شدت به درآمد این سرمایه‌گذاری وابسته‌ و برای تامین هزینه‌های معمول خود به این سرمایه‌گذاری متکی می¬باشد.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 112, QuestionId = 31, Text = "در دارائی‌های کم ریسک مانند سپرده بانکی یا اوراق بهادار با درآمد ثابت", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 113, QuestionId = 31, Text = "در دارائی‌های با ریسک متوسط مانند واحدهای صندوق‌های سرمایه‌گذاری مشترک", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 114, QuestionId = 31, Text = "در دارائی‌های با ریسک زیاد مانند سهام شرکتها", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 115, QuestionId = 31, Text = "در دارائی‌های با ریسک خیلی زیاد مانند قراردادهای آتی و اوراق اختیار معامله", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 116, QuestionId = 32, Text = "سرمایه‌گذاری در پروژه ای که قطعاً 20 میلیون تومان سود به همراه خواهد داشت.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 117, QuestionId = 32, Text = "سرمایه‌گذاری در پروژه ای که احتمال دارد شرکت 40 میلیون تومان سود کسب کند یا امکان دارد هیچ سودی به دست نیاورد.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 118, QuestionId = 32, Text = "سرمایه‌گذاری در پروژه ای که احتمال دارد شرکت 80 میلیون تومان سود کسب کند یا امکان دارد 40 میلیون تومان ضرر کند.", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 119, QuestionId = 32, Text = "سرمایه‌گذاری در پروژه ای که احتمال دارد شرکت 120 میلیون تومان سود کسب کند یا امکان دارد 80 میلیون تومان ضرر کند. ", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 120, QuestionId = 33, Text = "PORTFOLIO A", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 121, QuestionId = 33, Text = "PORTFOLIO B", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 122, QuestionId = 33, Text = "PORTFOLIO C", IsDeleted = false, Score = 0 },
            // new UserAnswerOption { Id = 123, QuestionId = 33, Text = "PORTFOLIO D", IsDeleted = false, Score = 0 }
            // );





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

            //modelBuilder.Entity<Admin>()
            //    .HasMany(u => u.Messages)
            //    .WithOne(r => r.AdminSender)
            //    .HasForeignKey(r => r.SenderId);

        }
    }

}
