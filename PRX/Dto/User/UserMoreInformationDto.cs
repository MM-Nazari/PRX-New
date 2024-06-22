using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserMoreInformationDto
    {

        //public int UserId { get; set; }
        public int RequestId { get; set; }

        [Required(ErrorMessage = "Info is required")]
        public string Info { get; set; }

        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
