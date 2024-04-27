﻿using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserAsset
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int AssetTypeId { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal AssetValue { get; set; } = decimal.Zero;
        [Required]
        [Range(0, 100)]
        public decimal AssetPercentage { get; set; } = decimal.Zero;
        public bool IsComplete { get; set; } = false;

        public User User { get; set; }

        public UserAssetType UserAssetType { get; set; }
    }
}
