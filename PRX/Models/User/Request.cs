using PRX.Models.Haghighi;
using PRX.Models.Hoghooghi;
using PRX.Models.Hoghooghis.Hoghooghi;
using PRX.Models.Quiz;
using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        public int UserId { get; set; }

        //[Required]
        public string RequestType { get; set; } // e.g., "Haghighi", "Hoghooghi"

        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public User User { get; set; }


        // New fields
        //[Required]
        public string TrackingCode { get; set; }

        //[Required]
        public DateTime RequestSentTime { get; set; }

        //[Required]
        public string BeneficiaryName { get; set; } // نام ذی نفع

        //[Required]
        public string RequestState { get; set; }



        //
        // Relations
        //

        // User

        public UserFinancialChanges UserFinancialChanges { get; set; }
        public UserFuturePlans UserFuturePlans { get; set; }
        public List<UserInvestmentExperience> UserInvestmentExperiences { get; set; }
        public List<UserAsset> UserAssets { get; set; }
        public List<UserWithdrawal> UserWithdrawals { get; set; }
        public List<UserDeposit> UserDeposits { get; set; }
        public List<UserDebt> UserDebts { get; set; }
        public UserInvestment UserInvestment { get; set; }
        public UserMoreInformation UserMoreInformations { get; set; }
        //public List<UserType> UserTypes { get; set; }
        //public UserState UserState { get; set; }
        public List<UserDocument> UserDocs { get; set; }
        //public UserReference UserReference { get; set; }
        public UserBankInfo UserBankInfos { get; set; }



        // Haghighi

        public HaghighiUserProfile HaghighiUserProfile { get; set; }
        public List<HaghighiUserRelationships> HaghighiUserRelationships { get; set; }
        public HaghighiUserFinancialProfile HaghighiUserFinancialProfiles { get; set; }
        public HaghighiUserEducationStatus HaghighiUserEducationStatus { get; set; }
        public List<HaghighiUserEmploymentHistory> EmploymentHistories { get; set; }



        // Hoghooghi

        public HoghooghiUser HoghooghiUser { get; set; }
        public List<HoghooghiUserBoardOfDirectors> HoghooghiUserBoardOfDirectors { get; set; }
        public List<HoghooghiUserInvestmentDepartmentStaff> HoghooghiUserInvestmentDepartmentStaff { get; set; }
        public List<HoghooghiUserCompaniesWithMajorInvestors> HoghooghiUserCompaniesWithMajorInvestors { get; set; }
        public List<HoghooghiUserAssetIncomeStatusTwoYearsAgo> HoghooghiUserAssetIncomeStatusTwoYearsAgos { get; set; }


        // Quiz

        public List<UserAnswer> UserAnswer { get; set; }
        public UserTestScore UserTestScore { get; set; }
    }
}