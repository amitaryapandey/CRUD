namespace QuotesApi.Models.DTOs
{
    public class StatisticsDto
    {
        public int TotalCourses { get; set; }
        public int TotalAssignments { get; set; }
        public int SubmittedAssignments { get; set; }
        public int GradedAssignments { get; set; }
        public string AverageScore { get; set; }
    }
}
