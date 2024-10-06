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

        public string Username { get; set; }

        public string PhoneNumber { get; set; }
        
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

        public List<UserType> UserTypes { get; set; }
        public UserState UserState { get; set; }

        public UserReference UserReference { get; set; }

        //// Log

        public List<UserLoginLog> UserLoginLogs { get; set; }


        // Ticket

        public List<PRX.Models.Ticket.Ticket> Tickets { get; set; }
        public List<PRX.Models.Ticket.Message> Messages { get; set; }
        public List<DataChangeLog> DataChangeLogs { get; set; }







    }

}
