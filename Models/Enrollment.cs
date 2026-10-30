using System;

namespace QuotesApi.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string Grade { get; set; }
        public string Status { get; set; } = "enrolled";
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
