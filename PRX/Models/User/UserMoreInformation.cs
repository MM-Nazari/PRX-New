using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserMoreInformation
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Info { get; set; }
        public bool IsComplete { get; set; } = false;

        public User User { get; set; }
    }
}
