namespace PRX.Models
{
    public class HaghighiUserEducationStatus
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LastDegree { get; set; }
        public string FieldOfStudy { get; set; }
        public int GraduationYear { get; set; }
        public string IssuingAuthority { get; set; }
    }
}
