using System.Collections.Generic;

namespace QuotesApi.Models.DTOs
{
    public class DashboardDto
    {
        public Student Student { get; set; }
        public List<CourseDto> Courses { get; set; }
        public List<AssignmentDto> Assignments { get; set; }
        public StatisticsDto Statistics { get; set; }
        public List<AssignmentDto> UpcomingAssignments { get; set; }
    }
}
