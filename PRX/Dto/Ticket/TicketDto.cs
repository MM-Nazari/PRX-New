using PRX.Models.Ticket;

namespace PRX.Dto.Ticket
{
    public class TicketDto
    {
        public int UserId { get; set; }
        public string TrackingCode { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Category { get; set; }

        public bool IsDeleted { get; set; }

    }
}
