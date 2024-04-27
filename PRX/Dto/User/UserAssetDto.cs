namespace PRX.Dto.User
{
    public class UserAssetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AssetTypeId { get; set; }
        public decimal AssetValue { get; set; }
        public decimal AssetPercentage { get; set; }
        public bool IsComplete { get; set; }
    }
}
