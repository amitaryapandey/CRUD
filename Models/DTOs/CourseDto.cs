using System;

namespace QuotesApi.Models.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public string Instructor { get; set; }
        public string Semester { get; set; }
        public string Grade { get; set; }
        public string Status { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
