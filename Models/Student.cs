using System;
using System.Collections.Generic;

namespace QuotesApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string StudentId { get; set; }
        public string Major { get; set; }
        public int Year { get; set; }
        public decimal? Gpa { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Submission> Submissions { get; set; }
    }
}
