using System;

namespace QuotesApi.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public int? PointsEarned { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "submitted";

        // Navigation properties
        public Assignment Assignment { get; set; }
        public Student Student { get; set; }
    }
}
