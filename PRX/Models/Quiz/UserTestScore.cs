using PRX.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Quiz
{
    public class UserTestScore
    {
        public int Id { get; set; }
        //[Required]
        //public int UserId { get; set; }
        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }
        [Required]
        public int QuizScore { get; set; }


        public bool IsDeleted { get; set; } = false;

        public Request Request { get; set; }
    }
}
