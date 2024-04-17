namespace PRX.Models
{
    public class HaghighiUserAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerOptionId { get; set; }
        public string AnswerText { get; set; }
    }

}
