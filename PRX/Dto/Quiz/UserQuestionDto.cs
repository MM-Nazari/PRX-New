namespace PRX.Dto.Quiz
{
    public class UserQuestionDto
    {
        public string Text { get; set; }

        public bool IsDeleted { get; set; }

        public string Type { get; set; }
        public int QuestionNumber { get; set; }
    }
}
