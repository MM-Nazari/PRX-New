namespace PRX.Models.Ticket
{
    public class Message
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        
        // Admin or User
        public string SenderType { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public int? UserId { get; set; }
        public int? AdminId { get; set; }

        public PRX.Models.User.User UserSender { get; set; }
        
        public Ticket Ticket { get; set; }
    }

}
