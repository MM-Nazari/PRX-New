using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.User
{
    public class UserInvestment
    {
        public int Id { get; set; }
        //[Required]
        //public int UserId { get; set; }
        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        //public User User { get; set; }
        public Request Request { get; set; }
    }

}
