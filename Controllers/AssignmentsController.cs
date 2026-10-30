using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AssignmentsController : ControllerBase
    {
        private readonly QuotesDbContext _context;

        public AssignmentsController(QuotesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all assignments with course information
        /// </summary>
        /// <returns>List of all assignments</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<AssignmentDto>> GetAssignments()
        {
            try
            {
                var assignments = _context.Assignments
                    .Include(a => a.Course)
                    .OrderBy(a => a.DueDate)
                    .Select(a => new AssignmentDto
                    {
                        Id = a.Id,
                        CourseId = a.CourseId,
                        Title = a.Title,
                        Description = a.Description,
                        DueDate = a.DueDate,
                        MaxPoints = a.MaxPoints,
                        CourseCode = a.Course.Code,
                        CourseName = a.Course.Name
                    })
                    .ToList();
                return Ok(assignments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving assignments", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific assignment by ID
        /// </summary>
        /// <param name="id">Assignment ID</param>
        /// <returns>Assignment object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Assignment> GetAssignment(int id)
        {
            try
            {
                var assignment = _context.Assignments
                    .Include(a => a.Course)
                    .FirstOrDefault(a => a.Id == id);
                    
                if (assignment == null)
                {
                    return NotFound(new { error = "Assignment not found" });
                }
                return Ok(assignment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving assignment", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new assignment
        /// </summary>
        /// <param name="assignment">Assignment object to create</param>
        /// <returns>Created assignment with ID</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Assignment> CreateAssignment([FromBody] Assignment assignment)
        {
            try
            {
                if (assignment == null)
                {
                    return BadRequest(new { message = "Assignment object is null" });
                }

                if (string.IsNullOrWhiteSpace(assignment.Title))
                {
                    return BadRequest(new { message = "Title is required" });
                }

                if (assignment.CourseId <= 0)
                {
                    return BadRequest(new { message = "Valid course ID is required" });
                }

                // Check if course exists
                if (!_context.Courses.Any(c => c.Id == assignment.CourseId))
                {
                    return BadRequest(new { message = "Course not found" });
                }

                assignment.CreatedAt = DateTime.UtcNow;
                _context.Assignments.Add(assignment);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetAssignment), new { id = assignment.Id }, assignment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error creating assignment", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing assignment
        /// </summary>
        /// <param name="id">Assignment ID to update</param>
        /// <param name="assignment">Updated assignment object</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateAssignment(int id, [FromBody] Assignment assignment)
        {
            try
            {
                if (assignment == null)
                {
                    return BadRequest(new { message = "Assignment object is null" });
                }

                var entity = _context.Assignments.Find(id);
                if (entity == null)
                {
                    return NotFound(new { message = $"Assignment with ID {id} not found" });
                }

                // Update fields
                if (!string.IsNullOrWhiteSpace(assignment.Title))
                    entity.Title = assignment.Title;
                if (!string.IsNullOrWhiteSpace(assignment.Description))
                    entity.Description = assignment.Description;
                if (assignment.DueDate != default(DateTime))
                    entity.DueDate = assignment.DueDate;
                if (assignment.MaxPoints > 0)
                    entity.MaxPoints = assignment.MaxPoints;

                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error updating assignment", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an assignment
        /// </summary>
        /// <param name="id">Assignment ID to delete</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteAssignment(int id)
        {
            try
            {
                var assignment = _context.Assignments.Find(id);
                if (assignment == null)
                {
                    return NotFound(new { message = $"Assignment with ID {id} not found" });
                }

                _context.Assignments.Remove(assignment);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error deleting assignment", error = ex.Message });
            }
        }
    }
}
