
using System.ComponentModel.DataAnnotations;

namespace PRX.Models.Quiz
{
    public class UserAnswerOption
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }

        public int Score { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;
        public UserQuestion UserQuestion { get; set; }
        //public List<HaghighiUserAnswer> HaghighiUserAnswers { get; set; }
        public UserAnswer UserAnswer { get; set; }
    }
}