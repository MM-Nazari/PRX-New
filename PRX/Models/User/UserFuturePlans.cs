using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRX.Utils;

namespace PRX.Models.User
{
    public class UserFuturePlans
    {
        public int Id { get; set; }
        //[Required]
        //public int UserId { get; set; }
        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }
        [Required]
        [DescriptionValidation]
        public string Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        //public User User { get; set; }
        public Request Request { get; set; }
    }
}
