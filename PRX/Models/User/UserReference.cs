﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.User
{
    public class UserReference
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [ForeignKey("ReferencedUser")]
        public int? ReferencedUser { get; set; }

        public bool IsDeleted { get; set; } = false;

        public User User { get; set; }

    }
}
