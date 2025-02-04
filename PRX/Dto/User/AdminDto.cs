﻿namespace PRX.Dto.User
{
    public class AdminDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Admin";
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

    }
}
