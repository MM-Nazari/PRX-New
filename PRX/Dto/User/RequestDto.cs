using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class RequestDto
    {
        
        public int UserId { get; set; }

        
        public string RequestType { get; set; } 

        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
