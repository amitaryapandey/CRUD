using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesApi.Data;
using QuotesApi.Models;

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly QuotesDbContext _context;

        public SubmissionsController(QuotesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all submissions
        /// </summary>
        /// <returns>List of all submissions</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Submission>> GetSubmissions()
        {
            try
            {
                var submissions = _context.Submissions
                    .Include(s => s.Assignment)
                    .Include(s => s.Student)
                    .OrderByDescending(s => s.SubmittedAt)
                    .ToList();
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving submissions", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific submission by ID
        /// </summary>
        /// <param name="id">Submission ID</param>
        /// <returns>Submission object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Submission> GetSubmission(int id)
        {
            try
            {
                var submission = _context.Submissions
                    .Include(s => s.Assignment)
                    .Include(s => s.Student)
                    .FirstOrDefault(s => s.Id == id);
                    
                if (submission == null)
                {
                    return NotFound(new { error = "Submission not found" });
                }
                return Ok(submission);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving submission", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new submission
        /// </summary>
        /// <param name="submission">Submission object to create</param>
        /// <returns>Created submission with ID</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Submission> CreateSubmission([FromBody] Submission submission)
        {
            try
            {
                if (submission == null)
                {
                    return BadRequest(new { message = "Submission object is null" });
                }

                if (submission.AssignmentId <= 0 || submission.StudentId <= 0)
                {
                    return BadRequest(new { error = "assignment_id and student_id are required" });
                }

                // Check if assignment exists
                if (!_context.Assignments.Any(a => a.Id == submission.AssignmentId))
                {
                    return BadRequest(new { message = "Assignment not found" });
                }

                // Check if student exists
                if (!_context.Students.Any(s => s.Id == submission.StudentId))
                {
                    return BadRequest(new { message = "Student not found" });
                }

                // Check for duplicate submission
                if (_context.Submissions.Any(s => s.AssignmentId == submission.AssignmentId && s.StudentId == submission.StudentId))
                {
                    return BadRequest(new { error = "Assignment already submitted" });
                }

                submission.SubmittedAt = DateTime.UtcNow;
                submission.Status = "submitted";
                _context.Submissions.Add(submission);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetSubmission), new { id = submission.Id }, submission);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error creating submission", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing submission (e.g., grade it)
        /// </summary>
        /// <param name="id">Submission ID to update</param>
        /// <param name="submission">Updated submission object</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateSubmission(int id, [FromBody] Submission submission)
        {
            try
            {
                if (submission == null)
                {
                    return BadRequest(new { message = "Submission object is null" });
                }

                var entity = _context.Submissions.Find(id);
                if (entity == null)
                {
                    return NotFound(new { message = $"Submission with ID {id} not found" });
                }

                // Update fields
                if (submission.PointsEarned.HasValue)
                {
                    entity.PointsEarned = submission.PointsEarned;
                    entity.Status = "graded";
                }
                if (!string.IsNullOrWhiteSpace(submission.Status))
                    entity.Status = submission.Status;

                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error updating submission", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a submission
        /// </summary>
        /// <param name="id">Submission ID to delete</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteSubmission(int id)
        {
            try
            {
                var submission = _context.Submissions.Find(id);
                if (submission == null)
                {
                    return NotFound(new { message = $"Submission with ID {id} not found" });
                }

                _context.Submissions.Remove(submission);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error deleting submission", error = ex.Message });
            }
        }
    }
}
