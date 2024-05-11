namespace PRX.Dto.User
{
    public class UserDocumentDto
    {
        public int UserId { get; set; }
        public string DocumentType { get; set; }
        //public string FilePath { get; set; }
        public IFormFile File { get; set; }
        public bool IsDeleted { get; set; }

    }
}
