using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserType
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Type { get; set; }

        public bool IsDeleted { get; set; } = false;
        public User User { get; set; }

    }
}
