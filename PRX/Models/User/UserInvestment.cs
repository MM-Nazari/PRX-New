using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserInvestment
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        public bool IsComplete { get; set; } = false;

        public User User { get; set; }
    }

}
