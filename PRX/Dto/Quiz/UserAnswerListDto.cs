namespace PRX.Dto.Quiz
{
    public class UserAnswerListDto
    {
        public int RequestId { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}
