using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using PRX.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.User
{

    public class UserFinancialChanges
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        [DescriptionValidation]
        public string Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public Request Request { get; set; }
    }

}
