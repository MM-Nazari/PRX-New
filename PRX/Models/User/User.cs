using System.ComponentModel.DataAnnotations;
using System.Net;
using PRX.Models.Haghighi;
using PRX.Models.Hoghooghi;
using PRX.Models.Hoghooghis.Hoghooghi;
using PRX.Models.Quiz;
using PRX.Models.Ticket;

namespace PRX.Models.User
{
    public class User
    {

        public int Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string ReferenceCode { get; set; } = string.Empty;

        public string Role { get; set; } = "User";
        public bool IsDeleted { get; set; } = false;


        // New fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthCertificateNumber { get; set; }


        //
        // Relations
        //

        // User

        //public UserFinancialChanges UserFinancialChanges { get; set; }
        //public UserFuturePlans UserFuturePlans { get; set; }
        //public List<UserInvestmentExperience> UserInvestmentExperiences { get; set; }
        //public List<UserAsset> UserAssets { get; set; }
        //public List<UserWithdrawal> UserWithdrawals { get; set; }
        //public List<UserDeposit> UserDeposits { get; set; }
        //public List<UserDebt> UserDebts { get; set; }
        //public UserInvestment UserInvestment { get; set; }
        //public UserMoreInformation UserMoreInformations { get; set; }
        public List<UserType> UserTypes { get; set; }
        public UserState UserState { get; set; }
        //public List<UserDocument> UserDocs { get; set; }
        public UserReference UserReference { get; set; }



        //// Haghighi

        //public HaghighiUserProfile HaghighiUserProfile { get; set; }
        //public List<HaghighiUserRelationships> HaghighiUserRelationships { get; set; }
        //public HaghighiUserFinancialProfile HaghighiUserFinancialProfiles { get; set; }
        //public HaghighiUserEducationStatus HaghighiUserEducationStatus { get; set; }
        //public List<HaghighiUserEmploymentHistory> EmploymentHistories { get; set; }
        //public HaghighiUserBankInfo HaghighiUserBankInfos { get; set; }


        //// Hoghooghi

        //public HoghooghiUser HoghooghiUser { get; set; }
        //public List<HoghooghiUserBoardOfDirectors> HoghooghiUserBoardOfDirectors { get; set; }
        //public List<HoghooghiUserInvestmentDepartmentStaff> HoghooghiUserInvestmentDepartmentStaff { get; set; }    
        //public List<HoghooghiUserCompaniesWithMajorInvestors> HoghooghiUserCompaniesWithMajorInvestors { get; set; }
        //public List<HoghooghiUserAssetIncomeStatusTwoYearsAgo> HoghooghiUserAssetIncomeStatusTwoYearsAgos { get; set; }



        //// Quiz

        //public List<UserAnswer> UserAnswer { get; set; }
        //public UserTestScore UserTestScore { get; set; }


        //// Log

        public List<UserLoginLog> UserLoginLogs { get; set; }


        // Ticket

        public List<PRX.Models.Ticket.Ticket> Tickets { get; set; }
        public List<PRX.Models.Ticket.Message> Messages { get; set; }







    }

}
