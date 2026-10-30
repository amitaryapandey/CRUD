using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesApi.Data;
using QuotesApi.Models;
using QuotesApi.Models.DTOs;

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly QuotesDbContext _context;

        public StudentsController(QuotesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>List of all students</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            try
            {
                var students = _context.Students
                    .OrderBy(s => s.Name)
                    .ToList();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving students", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific student by ID
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Student object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetStudent(int id)
        {
            try
            {
                var student = _context.Students.Find(id);
                if (student == null)
                {
                    return NotFound(new { error = "Student not found" });
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving student", error = ex.Message });
            }
        }

        /// <summary>
        /// Get complete dashboard data for a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Dashboard data including courses, assignments, and statistics</returns>
        [HttpGet("{id}/dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DashboardDto> GetStudentDashboard(int id)
        {
            try
            {
                var student = _context.Students.Find(id);
                if (student == null)
                {
                    return NotFound(new { error = "Student not found" });
                }

                // Get enrolled courses with grades
                var courses = _context.Enrollments
                    .Where(e => e.StudentId == id)
                    .Include(e => e.Course)
                    .Select(e => new CourseDto
                    {
                        Id = e.Course.Id,
                        Code = e.Course.Code,
                        Name = e.Course.Name,
                        Credits = e.Course.Credits,
                        Instructor = e.Course.Instructor,
                        Semester = e.Course.Semester,
                        Grade = e.Grade,
                        Status = e.Status,
                        EnrolledAt = e.EnrolledAt
                    })
                    .ToList();

                // Get all assignments for enrolled courses with submission status
                var enrolledCourseIds = courses.Select(c => c.Id).ToList();
                var assignments = _context.Assignments
                    .Where(a => enrolledCourseIds.Contains(a.CourseId))
                    .Include(a => a.Course)
                    .Select(a => new
                    {
                        Assignment = a,
                        Submission = _context.Submissions
                            .FirstOrDefault(s => s.AssignmentId == a.Id && s.StudentId == id)
                    })
                    .AsEnumerable()
                    .Select(x => new AssignmentDto
                    {
                        Id = x.Assignment.Id,
                        CourseId = x.Assignment.CourseId,
                        Title = x.Assignment.Title,
                        Description = x.Assignment.Description,
                        DueDate = x.Assignment.DueDate,
                        MaxPoints = x.Assignment.MaxPoints,
                        CourseCode = x.Assignment.Course.Code,
                        CourseName = x.Assignment.Course.Name,
                        PointsEarned = x.Submission?.PointsEarned,
                        SubmittedAt = x.Submission?.SubmittedAt,
                        SubmissionStatus = x.Submission?.Status
                    })
                    .OrderBy(a => a.DueDate)
                    .ToList();

                // Calculate statistics
                var submittedAssignments = assignments.Count(a => a.SubmittedAt != null);
                var gradedAssignments = assignments.Count(a => a.PointsEarned != null);
                var gradedScores = assignments
                    .Where(a => a.PointsEarned != null && a.MaxPoints > 0)
                    .Select(a => (decimal)a.PointsEarned.Value / a.MaxPoints * 100)
                    .ToList();
                var averageScore = gradedScores.Any() 
                    ? gradedScores.Average().ToString("F1") 
                    : "0.0";

                var statistics = new StatisticsDto
                {
                    TotalCourses = courses.Count,
                    TotalAssignments = assignments.Count,
                    SubmittedAssignments = submittedAssignments,
                    GradedAssignments = gradedAssignments,
                    AverageScore = averageScore
                };

                // Get upcoming assignments (not submitted and due in the future)
                var upcomingAssignments = assignments
                    .Where(a => a.SubmittedAt == null && a.DueDate > DateTime.UtcNow)
                    .OrderBy(a => a.DueDate)
                    .ToList();

                var dashboard = new DashboardDto
                {
                    Student = student,
                    Courses = courses,
                    Assignments = assignments,
                    Statistics = statistics,
                    UpcomingAssignments = upcomingAssignments
                };

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Create a new student
        /// </summary>
        /// <param name="student">Student object to create</param>
        /// <returns>Created student with ID</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> CreateStudent([FromBody] Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest(new { message = "Student object is null" });
                }

                if (string.IsNullOrWhiteSpace(student.Name))
                {
                    return BadRequest(new { message = "Name is required" });
                }

                if (string.IsNullOrWhiteSpace(student.Email))
                {
                    return BadRequest(new { message = "Email is required" });
                }

                if (string.IsNullOrWhiteSpace(student.StudentId))
                {
                    return BadRequest(new { message = "Student ID is required" });
                }

                // Check for duplicate email or student ID
                if (_context.Students.Any(s => s.Email == student.Email))
                {
                    return BadRequest(new { message = "Email already exists" });
                }

                if (_context.Students.Any(s => s.StudentId == student.StudentId))
                {
                    return BadRequest(new { message = "Student ID already exists" });
                }

                student.CreatedAt = DateTime.UtcNow;
                _context.Students.Add(student);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error creating student", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing student
        /// </summary>
        /// <param name="id">Student ID to update</param>
        /// <param name="student">Updated student object</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest(new { message = "Student object is null" });
                }

                var entity = _context.Students.Find(id);
                if (entity == null)
                {
                    return NotFound(new { message = $"Student with ID {id} not found" });
                }

                // Update fields
                if (!string.IsNullOrWhiteSpace(student.Name))
                    entity.Name = student.Name;
                if (!string.IsNullOrWhiteSpace(student.Email))
                    entity.Email = student.Email;
                if (!string.IsNullOrWhiteSpace(student.Major))
                    entity.Major = student.Major;
                if (student.Year > 0)
                    entity.Year = student.Year;
                if (student.Gpa.HasValue)
                    entity.Gpa = student.Gpa;

                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error updating student", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a student
        /// </summary>
        /// <param name="id">Student ID to delete</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int id)
        {
            try
            {
                var student = _context.Students.Find(id);
                if (student == null)
                {
                    return NotFound(new { message = $"Student with ID {id} not found" });
                }

                _context.Students.Remove(student);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error deleting student", error = ex.Message });
            }
        }
    }
}
