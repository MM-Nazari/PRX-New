namespace PRX.Utils
{
    public class ResponseMessages
    {
        //
        // 200
        //

        public const string OK = "عملیات با موفقیت انجام شد";
        public const string test = "";

        // User

        public const string InvalidId = "شناسه کاربری اشتباه است";

        public const string PhoneExistanseTrue = "yes";
        public const string PhoneExistanseFalse = "no";

        public const string UsersPhoneExists = " این شماره تلفن قبلا ثبت شده است ";

        public const string UserNotExists = "حساب کاربری با این شماره تلفن وجود ندارد ";
        public const string PasswordIncorrect = "رمز عبور اشتباه است";

        public const string UserNotFound = "کاربر با این مشخصات وجود ندارد";
        public const string Unauthorized = "شناسه کاربری با شناسه توکن مطابقت ندارد ";
        public const string Forbidden = "امکان دسترسی بدلیل عدم تطابق نقش وجود ندارد ";

        


        // User Asset

        public const string UserAssetNotFound = "دارایی متناسب با کاربر یافت نشد";

        // User Asset Type

        public const string UserAssetTypeNotFound = "نوع دارایی متناسب با کاربر یافت نشد";

        // User Debt 

        public const string UserDebtNotFound = "بدهی متناسب با کاربر یافت نشد";

        // User Deposit 

        public const string UserDepositNotFound = "سپرده متناسب با کاربر یافت نشد";

        // User Document

        public const string UserDocNotFound = "مدرکی متناسب با کاربر یافت نشد";
        public const string UserFileFormatConfliction = "فرمت فایل تعریف شده نیست";
        public const string UserFilePathConfliction = "مسیر فایل ذخیره شده با اطلاعات کاربر تطابق ندارد";
        public const string UserFileNotFound = "مدرک افزوده نشده است";

        // User Financial Change

        public const string UserFinancialChangeNotFound = "تغییر مالی متناسب با کاربر یافت نشد";

        // User Future Plans

        public const string UserFuturePlanNotFound = " برنامه آینده متناسب با کاربر یافت نشد";

        // User Investment
        public const string UserInvestmentNotFound = "سرمایه گذاری متناسب با کاربر یافت نشد";

        // User Investment Experience
        public const string UserInvestmentExperienceNotFound = "تجربه سرمایه گذاری متناسب با کاربر یافت نشد";

        // User More Information
        public const string UserMoreInfoNotFound = "اطلاعات اضافی متناسب با کاربر یافت نشد";

        // User State
        public const string UserStateNotFound = "وضعیت متناسب با کاربر یافت نشد";

        // User Type
        public const string UserTypeNotFound = "نوع متناسب با کاربر یافت نشد";

        // User Withdrawl 
        public const string UserWithdrawlNotFound = "برداشت متناسب با کاربر یافت نشد";

        // User References
        public const string UserRefernceCodeFormatIsInvalid = "فرمت کد معرف صحیح نمیباشد ";
        public const string UserRefernceCodeIsInvalid = "کد معرف صحیح نیست ";



        // Haghighi User Education Status
        public const string HaghighiUserEducationStatusNotFound = "وضعیت تحصیلی متناسب با کاربر حقیقی یافت نشد";

        // Haghighi User Employment History
        public const string HaghighiUserEmploymentHistoryNotFound = "سابقه اشتغال متناسب با کاربر حقیقی یافت نشد";

        // Haghighi User Financial Profile
        public const string HaghighiUserFinancialProfileNotFound = "پروفایل مالی متناسب با کاربر حقیقی یافت نشد ";

        // Haghighi User Profile
        public const string HaghighiUserProfileNotFound = "پروفایل متناسب با کاربر حقیقی یافت نشد";
        public const string HaghighiUserProfileDuplicateBirthCertificate = "شماره ملی وارد شده تکراری است";

        // Haghighi User Relationship 
        public const string HaghighiUserRelationNotFound = "رابطه متناسب با کاربر حقیقی یافت نشد";

        // Haghighi User Bank Info
        public const string HaghighiUserBankInfoNotFound = "اطلاعات بانکی متناسب با کاربر حقیقی یافت نشد";



        // Hoghooghi User Asset Income
        public const string HoghooghiAssetIncomeNotfound = "دارایی طی دوسال گذشته متناسب با کاربر حقوقی یافت نشد";

        // Hoghooghi User Directors
        public const string HoghooghiDrectorsNotFound = "ترکیب هیأت مدیره متناسب با کاربر حقوقی یافت نشد";

        // Hoghooghi User Companies
        public const string HoghooghiCompaniesNotFound = "شرکت متناسب با کاربر حقوقی یافت نشد";

        // Hoghooghi User
        public const string HoghooghiUserNotFound = "کاربر حقوقی با مشخصات داده شده دیافت نشد";

        // Hoghooghi User Department
        public const string HoghooghiDepartmentNotFound = "هیأت کارشناس و مدیران متناسب با کاربر حقوقی یافت نشد";



        // Quiz Answer
        public const string QuizAnswerNotFound = "جواب متناسب با کاربر یافت نشد";
        public const string QuizAnswerIsNull = "جواب کاربر نمیتواند خالی باشد";

        // Quiz Answer Option
        public const string QuizAnswerOptionNotFound = "گظینه انتخاب شده متناسب با کاربر یافت نشد";

        // Quiz Question
        public const string QuizQuestionNotFound = "سوال متناسب با کاربر یافت نشد";

        // Quiz Score
        public const string QuizScoreNotFound = "نمره متناسب با کاربر یافت نشد";



        // Admin
        public const string AdminNotFound = "ادمین با مشخصات داده شده یافت نشد";



        // 
        public const string InternalServerError = "ارور داخلی سرور";
    }
}
