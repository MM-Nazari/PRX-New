
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRX.Models.User;

namespace PRX.Models.Quiz
{
    public class UserAnswer
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }
        public int AnswerOptionId { get; set; }
        public string? AnswerText { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Request Request { get; set; }
        public UserAnswerOption answerOption { get; set; }
    }
}
