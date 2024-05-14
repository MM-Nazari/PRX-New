using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserDocument
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public bool IsDeleted { get; set; } = false;
        public User User { get; set; }

    }
}
