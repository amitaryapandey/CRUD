using System;

namespace QuotesApi.Models.DTOs
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int MaxPoints { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int? PointsEarned { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string SubmissionStatus { get; set; }
    }
}
