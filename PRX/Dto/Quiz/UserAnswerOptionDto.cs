namespace PRX.Dto.Quiz
{
    public class UserAnswerOptionDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }

        public bool IsDeleted { get; set; }
    }
}
