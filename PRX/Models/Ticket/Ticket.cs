using System.ComponentModel.DataAnnotations;
using PRX.Models.User;

namespace PRX.Models.Ticket
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TrackingCode { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Category { get; set; }

        public bool IsDeleted { get; set; } = false;

        public PRX.Models.User.User User { get; set; }
        public List<Message> Messages { get; set; }
    }

}
