using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserLoginLog
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public DateTime LoginTime { get; set; }
        public string Role { get; set; }
        public User User { get; set; }
    }
}
