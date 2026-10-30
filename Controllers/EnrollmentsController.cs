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
    public class EnrollmentsController : ControllerBase
    {
        private readonly QuotesDbContext _context;

        public EnrollmentsController(QuotesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all enrollments
        /// </summary>
        /// <returns>List of all enrollments</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Enrollment>> GetEnrollments()
        {
            try
            {
                var enrollments = _context.Enrollments
                    .Include(e => e.Student)
                    .Include(e => e.Course)
                    .OrderByDescending(e => e.EnrolledAt)
                    .ToList();
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving enrollments", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific enrollment by ID
        /// </summary>
        /// <param name="id">Enrollment ID</param>
        /// <returns>Enrollment object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Enrollment> GetEnrollment(int id)
        {
            try
            {
                var enrollment = _context.Enrollments
                    .Include(e => e.Student)
                    .Include(e => e.Course)
                    .FirstOrDefault(e => e.Id == id);
                    
                if (enrollment == null)
                {
                    return NotFound(new { error = "Enrollment not found" });
                }
                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving enrollment", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new enrollment
        /// </summary>
        /// <param name="enrollment">Enrollment object to create</param>
        /// <returns>Created enrollment with ID</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Enrollment> CreateEnrollment([FromBody] Enrollment enrollment)
        {
            try
            {
                if (enrollment == null)
                {
                    return BadRequest(new { message = "Enrollment object is null" });
                }

                if (enrollment.StudentId <= 0 || enrollment.CourseId <= 0)
                {
                    return BadRequest(new { message = "Valid student ID and course ID are required" });
                }

                // Check if student exists
                if (!_context.Students.Any(s => s.Id == enrollment.StudentId))
                {
                    return BadRequest(new { message = "Student not found" });
                }

                // Check if course exists
                if (!_context.Courses.Any(c => c.Id == enrollment.CourseId))
                {
                    return BadRequest(new { message = "Course not found" });
                }

                // Check for duplicate enrollment
                if (_context.Enrollments.Any(e => e.StudentId == enrollment.StudentId && e.CourseId == enrollment.CourseId))
                {
                    return BadRequest(new { message = "Student already enrolled in this course" });
                }

                enrollment.EnrolledAt = DateTime.UtcNow;
                enrollment.Status = "enrolled";
                _context.Enrollments.Add(enrollment);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, enrollment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error creating enrollment", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing enrollment (e.g., assign grade)
        /// </summary>
        /// <param name="id">Enrollment ID to update</param>
        /// <param name="enrollment">Updated enrollment object</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateEnrollment(int id, [FromBody] Enrollment enrollment)
        {
            try
            {
                if (enrollment == null)
                {
                    return BadRequest(new { message = "Enrollment object is null" });
                }

                var entity = _context.Enrollments.Find(id);
                if (entity == null)
                {
                    return NotFound(new { message = $"Enrollment with ID {id} not found" });
                }

                // Update fields
                if (!string.IsNullOrWhiteSpace(enrollment.Grade))
                    entity.Grade = enrollment.Grade;
                if (!string.IsNullOrWhiteSpace(enrollment.Status))
                    entity.Status = enrollment.Status;

                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error updating enrollment", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an enrollment
        /// </summary>
        /// <param name="id">Enrollment ID to delete</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteEnrollment(int id)
        {
            try
            {
                var enrollment = _context.Enrollments.Find(id);
                if (enrollment == null)
                {
                    return NotFound(new { message = $"Enrollment with ID {id} not found" });
                }

                _context.Enrollments.Remove(enrollment);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error deleting enrollment", error = ex.Message });
            }
        }
    }
}
