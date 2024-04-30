using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using PRX.Utils;

namespace PRX.Models.User
{

    public class UserFinancialChanges
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [DescriptionValidation]
        public string Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public User User { get; set; }
    }

}
