using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class DataChangeLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthCertificateNumber { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public string Type { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }


        public User User { get; set; }

    }
}
