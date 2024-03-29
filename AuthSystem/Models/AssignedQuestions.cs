﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthSystem.Models
{
    public class AssignedQuestions
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ApplicationId { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey("TestDetailId")]
        public TestDetail TestDetail { get; set; }

        public int TestDetailId { get; set; }

        public string? UserResponse { get; set; }
        public TimeSpan? StartTime { get; set; }
    }
}