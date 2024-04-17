namespace PRX.Models
{
    public class HaghighiUserEmploymentHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string EmployerLocation { get; set; }
        public string MainActivity { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string WorkAddress { get; set; }
        public string WorkPhone { get; set; }
    }
}
