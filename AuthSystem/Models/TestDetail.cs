﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthSystem.Models
{
    public class TestDetail
    {
        [Key]
        public int TDId { get; set; }

        [ForeignKey("Test")]
        public int Id { get; set; }
        public Test Test { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int Percentage { get; set; }

    }
}
