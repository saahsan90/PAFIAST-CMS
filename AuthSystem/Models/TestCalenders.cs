﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthSystem.Models
{
    public class TestCalenders
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Test")]
        public int TestId { get; set; }

        public Test Test { get; set; }

        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        [ForeignKey("TestCenter")]
        public int TestCenterId { get; set; }

        public TestCenters TestCenter { get; set; }

        public string CalendarToken { get; set; }

        public int Code { get; set; }
    }
}