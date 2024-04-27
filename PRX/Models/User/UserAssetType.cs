using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserAssetType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<UserAsset> UserAssets { get; set; }
    }

}
