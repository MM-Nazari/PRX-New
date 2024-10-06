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
        public const string UsersBirthCertificateExists = "این شماره ملی قبلا ثبت شده است";

        public const string UserNotExists = "حساب کاربری با این شماره تلفن وجود ندارد ";
        public const string PasswordIncorrect = "رمز عبور اشتباه است";

        public const string UserNotFound = "کاربر با این مشخصات وجود ندارد";
        public const string Unauthorized = "شناسه کاربری با شناسه توکن مطابقت ندارد ";
        public const string Forbidden = "امکان دسترسی بدلیل عدم تطابق نقش وجود ندارد ";

        public const string UsersUsernameExists = "نام کاربری تکراری است";

        public const string AdminNotExists = "ادمین با مشخصات داده شده یافت نشد";

        public const string InvalidCurrentPassword = "رمز عبور فعلی اشتباه است";


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
        public const string UserFinancialChangeDuplicate = "تغییر مالی متناسب این کاربر قبلا ثبت شده است";

        // User Future Plans

        public const string UserFuturePlanNotFound = " برنامه آینده متناسب با کاربر یافت نشد";
        public const string UserFuturePlanDuplicate = " برنامه آینده این کاربر قبلا ثبت شده است";

        // User Investment
        public const string UserInvestmentNotFound = "سرمایه گذاری متناسب با کاربر یافت نشد";
        public const string UserInvestmentDuplicate = "سرمایه گذاری متناسب با این کاربر قبلا ثبت شده است";

        // User Investment Experience
        public const string UserInvestmentExperienceNotFound = "تجربه سرمایه گذاری متناسب با کاربر یافت نشد";

        // User More Information
        public const string UserMoreInfoNotFound = "اطلاعات اضافی متناسب با کاربر یافت نشد";
        public const string UserMoreInfoDuplicate = "اطلاعات اضافی متناسب با این کاربر قبلا ثبت شده است";

        // User State
        public const string UserStateNotFound = "وضعیت متناسب با کاربر یافت نشد";

        // User Type
        public const string UserTypeNotFound = "نوع متناسب با کاربر یافت نشد";

        // User Withdrawl 
        public const string UserWithdrawlNotFound = "برداشت متناسب با کاربر یافت نشد";

        // User References
        public const string UserRefernceCodeFormatIsInvalid = "فرمت کد معرف صحیح نمیباشد ";
        public const string UserRefernceCodeIsInvalid = "کد معرف صحیح نیست ";

        // User Bank Info
        public const string UserBankInfoNotFound = "اطلاعات بانکی متناسب با کاربر حقیقی یافت نشد";
        public const string UserBankInfoDuplicate = "اطلاعات بانکی متناسب این کاربر حقیقی قبلا ثبت شده است";



        // Haghighi User Education Status
        public const string HaghighiUserEducationStatusNotFound = "وضعیت تحصیلی متناسب با کاربر حقیقی یافت نشد";
        public const string HaghighiUserEducationStatusDuplicate = "وضعیت تحصیلی متناسب با این کاربر حقیقی قبلا ثبت شده است";

        // Haghighi User Employment History
        public const string HaghighiUserEmploymentHistoryNotFound = "سابقه اشتغال متناسب با کاربر حقیقی یافت نشد";

        // Haghighi User Financial Profile
        public const string HaghighiUserFinancialProfileNotFound = "پروفایل مالی متناسب با کاربر حقیقی یافت نشد ";
        public const string HaghighiUserFinancialProfileDuplicate = "پروفایل مالی متناسب با این کاربر حقیقی قبلا ثبت شده است ";

        // Haghighi User Profile
        public const string HaghighiUserProfileNotFound = "پروفایل متناسب با کاربر حقیقی یافت نشد";
        public const string HaghighiUserProfileDuplicateBirthCertificate = "شماره ملی وارد شده تکراری است";
        public const string HaghighiUserProfileDuplicate = "پروفایل متناسب با این کاربر حقیقی قبلا ثبت شده است";

        // Haghighi User Relationship 
        public const string HaghighiUserRelationNotFound = "رابطه متناسب با کاربر حقیقی یافت نشد";





        // Hoghooghi User Asset Income
        public const string HoghooghiAssetIncomeNotfound = "دارایی طی دوسال گذشته متناسب با کاربر حقوقی یافت نشد";

        // Hoghooghi User Directors
        public const string HoghooghiDrectorsNotFound = "ترکیب هیأت مدیره متناسب با کاربر حقوقی یافت نشد";

        // Hoghooghi User Companies
        public const string HoghooghiCompaniesNotFound = "شرکت متناسب با کاربر حقوقی یافت نشد";

        // Hoghooghi User
        public const string HoghooghiUserNotFound = "کاربر حقوقی با مشخصات داده شده دیافت نشد";
        public const string HoghooghiUserDuplicate = "کاربر حقوقی با مشخصات داده شده قبلا ثبت شده است";

        // Hoghooghi User Department
        public const string HoghooghiDepartmentNotFound = "هیأت کارشناس و مدیران متناسب با کاربر حقوقی یافت نشد";



        // Quiz Answer
        public const string QuizAnswerNotFound = "جواب متناسب با کاربر یافت نشد";
        public const string QuizAnswerIsNull = "جواب کاربر نمیتواند خالی باشد";
        public const string DuplicateAnswerOption = "این گزینه قبلا برای کاربر ثبت شده است";

        // Quiz Answer Option
        public const string QuizAnswerOptionNotFound = "گظینه انتخاب شده متناسب با کاربر یافت نشد";

        // Quiz Question
        public const string QuizQuestionNotFound = "سوال متناسب با کاربر یافت نشد";
        public const string QuizQuestionFilterNotFound = "سوال متناسب با فیلتر داده شده یافت نشد";

        // Quiz Score
        public const string QuizScoreNotFound = "نمره متناسب با کاربر یافت نشد";



        // Admin
        public const string AdminNotFound = "ادمین با مشخصات داده شده یافت نشد";

        // Ticket 
        public const string TicketNotFound = "تیکت متناسب با کاربر یافت نشد";
        public const string MessageNotFound = "پیام متناسب با کاربر یا ادمین یافت نشد";


        // Token 

        public const string TokenIsInvalid = "فرمت توکن اشتباه است";


        // OTP

        public const string OTPMobileNotFound = "شماره موبایل وارد نشده است";
        public const string OTPCouldntBeSent = "بدلیل ناتوانی برقراری ارتباط با سرور امکان ارسال پیامک وجود ندارد ";
        public const string OTPVerificationFailed = "کد وارد شده صحیح نمیباشد ";
        public const string OTPVerificationSucceded = "احراز هویت با سرویس پیامک کوتاه با موفقیت انجام شد ";


        // National Code

        public const string NationalCodeNotFound = "کد ملی وارد نشده است";
        public const string MobileNotFound = " شماره موبایل وارد نشده است ";


        // logout

        public const string LogoutSuccessfully = "عملیات خروج کاربر از سامانه با موفقیت انجام شد";
        public const string LogoutNotLoggedin = "کاربر قبلا به سامانه وارد نشده است";
        public const string LogoutInvalidToken = "توکن نامعتبر است";


        // Request

        public const string RequestNotFound = "درخواست متناسب با اطلاعات داده شده پیدا نشد";
        public const string UserRequestNotFound = "درخواست متناسب با کاربر پیدا نشد";
        public const string InvalidTrackingCode = "کد رهگیری نامعتبر است";
        public const string MaximumRequestTypeHaghighi = "شما حداکثر مجاز به ثبت یک درخواست حقیقی هستید";
        public const string MaximumRequestTypeHoghooghi = "شما حداکثر مجاز به ثبت 3 درخواست حقوقی هستید";


        // Report

        public const string RequestForUserNotFound = "درخواست متناسب با کاربر یافت نشد";

        // 500
        public const string InternalServerError = "ارور داخلی سرور";
    }
}
