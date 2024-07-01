namespace PRX.Dto.Quiz
{
    public class UserAnswerDto
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int AnswerOptionId { get; set; }
        public string AnswerText { get; set; }

        public bool IsDeleted { get; set; }
    }
}
