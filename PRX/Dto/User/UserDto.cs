using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserDto
    {
        public int Id { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Password { get; set; } // Add Password field
        
        public string ReferenceCode { get; set; } = string.Empty;

       // public int otp {  get; set; }

        public string Role { get; set; } = "User";
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
