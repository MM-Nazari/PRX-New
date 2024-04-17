namespace PRX.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int ReferenceCode { get; set; }
        public string Type { get; set; }

        // Navigation property for one-to-one relationship with HaghighiUserProfile
        public HaghighiUserProfile HaghighiUserProfile { get; set; }


        // Navigation property for one-to-many relationship with HaghighiUserRelationships
        public List<HaghighiUserRelationships> HaghighiUserRelationships { get; set; }

    }

}
