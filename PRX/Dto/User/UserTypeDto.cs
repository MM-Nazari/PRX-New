namespace PRX.Dto.User
{
    public class UserTypeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }

        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
