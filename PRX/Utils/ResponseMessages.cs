namespace PRX.Utils
{
    public class ResponseMessages
    {
        //
        // 200
        //

        // User

        public const string InvalidId = "شناسه کاربری اشتباه است";

        public const string PhoneExistanseTrue = "این شماره تلفن دارای حساب کاربری میباشد";
        public const string PhoneExistanseFalse = "برای این شماره تلفن حساب کاربری تعریف نشده است";

        public const string UsersPhoneExists = " این شماره تلفن قبلا ثبت شده است ";

        public const string UserNotExists = "حساب کاربری با این شماره تلفن وجود ندارد ";
        public const string PasswordIncorrect = "رمز عبور استباه است";

        public const string UserNotFound = "کاربر با این مشخصات وجود ندارد";
        public const string Unauthorized = "شناسه کاربری با شناسه توکن مطابقت ندارد ";
        public const string Forbidden = "امکان دسترسی بدلیل عدم تطابق نقش وجود ندارد ";

        public const string OK = "عملیات با موفقیت انجام شد";

        public const string test = "";





        //
        // 201
        //



        //
        // 400
        //



        //
        // 401
        //



        //
        // 403
        //


        //
        // 404
        //



        //
        // 500
        //

        public const string InternalServerError = "ارور داخلی سرور";
    }
}
