namespace PRX.Dto.Haghighi
{
    public class HaghighiUserEmploymentHistoryDto
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string EmployerLocation { get; set; }
        public string MainActivity { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string WorkAddress { get; set; }
        public string WorkPhone { get; set; }


        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
