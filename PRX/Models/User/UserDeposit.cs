using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserDeposit
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal DepositAmount { get; set; }
        [Required]
        public DateTime DepositDate { get; set; }
        [Required]
        public string DepositSource { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public User User { get; set; }
    }
}
