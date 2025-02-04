﻿using PRX.Models.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Haghighi
{
    public class HaghighiUserEmploymentHistory
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        public string EmployerLocation { get; set; }

        [Required]
        public string MainActivity { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }


        public string WorkAddress { get; set; }


        public string WorkPhone { get; set; }

        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public Request Request { get; set; }
    }
}
