using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserDto
    {
        public int Id { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Password { get; set; } // Add Password field
        
        public int? ReferenceCode { get; set; }
    }
}
