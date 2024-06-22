namespace PRX.Dto.User
{
    public class UserAssetDto
    {
        
        //public int UserId { get; set; }
        public int RequestId { get; set; }
        public int AssetTypeId { get; set; }
        public decimal AssetValue { get; set; }
        public decimal AssetPercentage { get; set; }

        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
