using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.User
{
    public class UserWithdrawal
    {
        public int Id { get; set; }


        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal WithdrawalAmount { get; set; }

        [Required]
        public DateTime WithdrawalDate { get; set; }

        [Required]
        public string WithdrawalReason { get; set; }

        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public Request Request { get; set; }
    }

}
