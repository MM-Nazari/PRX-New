namespace PRX.Models
{
    public class HaghighiUserUserAsset
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AssetTypeId { get; set; }
        public float AssetValue { get; set; }
        public float AssetPercentage { get; set; }
    }
}
