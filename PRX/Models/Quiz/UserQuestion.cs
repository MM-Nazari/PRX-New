
using System.ComponentModel.DataAnnotations;

namespace PRX.Models.Quiz
{
    public class UserQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public bool IsDeleted { get; set; } = false;

        public List<UserAnswerOption> AnswerOptions { get; set; }
        //public HaghighiUserAnswer UserAnswers { get; set; }
    }
}