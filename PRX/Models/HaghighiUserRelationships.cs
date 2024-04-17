namespace PRX.Models
{
    public class HaghighiUserRelationships
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string RelationshipStatus { get; set; }
        public int BirthYear { get; set; }
        public string EducationLevel { get; set; }
        public string EmploymentStatus { get; set; }
        public float AverageMonthlyIncome { get; set; }
        public float AverageMonthlyExpense { get; set; }
        public float ApproximateAssets { get; set; }
        public float ApproximateLiabilities { get; set; }


        // Navigation property for one-to-one relationship with User
        public User User { get; set; }
    }

}
