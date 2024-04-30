using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserMoreInformationDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Info is required")]
        public string Info { get; set; }

        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
