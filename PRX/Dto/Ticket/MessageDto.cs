namespace PRX.Dto.Ticket
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }

        // Admin or User
        public string SenderType { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;


        public int? UserId { get; set; }
        public int? AdminId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
