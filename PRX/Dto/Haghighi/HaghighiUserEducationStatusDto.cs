namespace PRX.Dto.Haghighi
{
    public class HaghighiUserEducationStatusDto
    {
        public int UserId { get; set; }
        public string LastDegree { get; set; }
        public string FieldOfStudy { get; set; }
        public int GraduationYear { get; set; }
        public string IssuingAuthority { get; set; }


        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
