using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.User
{
    public class UserState
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        //[Required]
        //[ForeignKey("Request")]
        //public int RequestId { get; set; }
        [Required]
        public string State { get; set; }

        public bool IsDeleted { get; set; } = false;

        public User User { get; set; }
        //public Request Request { get; set; }
    }
}
