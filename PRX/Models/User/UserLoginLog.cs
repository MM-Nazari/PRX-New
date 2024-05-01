namespace PRX.Models.User
{
    public class UserLoginLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // This can store the user ID for reference
        public DateTime LoginTime { get; set; }
        // You can include other information related to the login, such as IP address, device info, etc.

        public User User { get; set; }
    }
}
