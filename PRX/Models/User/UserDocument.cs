using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.User
{
    public class UserDocument
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }
        //public int UserId { get; set; }
        [Required]
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public bool IsDeleted { get; set; } = false;
        //public User User { get; set; }
        public Request Request { get; set; }

    }
}
