using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserLoginDto
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
