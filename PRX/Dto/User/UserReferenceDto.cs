namespace PRX.Dto.User
{
    public class UserReferenceDto
    {
        public int UserId { get; set; }

        public int? ReferencedUser { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
