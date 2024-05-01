
using System.ComponentModel.DataAnnotations;
using PRX.Models.User;

namespace PRX.Models.Quiz
{
    public class UserAnswer
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        //public int QusetionId { get; set; }

        public int AnswerOptionId { get; set; }
        public string? AnswerText { get; set; }

        public bool IsDeleted { get; set; } = false;

        public User.User User { get; set; }
        public UserAnswerOption answerOption { get; set; }
    }
}
