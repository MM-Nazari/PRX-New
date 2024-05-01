namespace PRX.Dto.Quiz
{
    public class UserTestScoreDto
    {
        public int UserId { get; set; }
        public int QuizScore { get; set; }

        public bool IsDeleted { get; set; }
    }
}
