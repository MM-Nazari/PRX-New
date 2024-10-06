namespace PRX.Dto.User
{
    public class UserDocumentDto
    {
        public int RequestId { get; set; }
        public string DocumentType { get; set; }
        public IFormFile File { get; set; }
        public bool IsDeleted { get; set; }

    }
}
