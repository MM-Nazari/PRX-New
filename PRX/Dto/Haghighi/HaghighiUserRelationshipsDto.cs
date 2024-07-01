namespace PRX.Dto.Haghighi
{
    public class HaghighiUserRelationshipsDto
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string FullName { get; set; }
        public string RelationshipStatus { get; set; }
        public int BirthYear { get; set; }
        public string EducationLevel { get; set; }
        public string EmploymentStatus { get; set; }
        public decimal AverageMonthlyIncome { get; set; }
        public decimal AverageMonthlyExpense { get; set; }
        public decimal ApproximateAssets { get; set; }
        public decimal ApproximateLiabilities { get; set; }

        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
