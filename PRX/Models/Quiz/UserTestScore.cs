using System.ComponentModel.DataAnnotations;

namespace PRX.Models.Quiz
{
    public class UserTestScore
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Score { get; set; }


        public bool IsDeleted { get; set; } = false;

        public PRX.Models.User.User User { get; set; }
    }
}
