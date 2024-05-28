﻿namespace PRX.Models.Admin
{
    public class Admin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Admin";

        public List<PRX.Models.Ticket.Message> Messages { get; set; }
    }
}
