﻿using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserState
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string State { get; set; }

        public User User { get; set; }
    }
}
