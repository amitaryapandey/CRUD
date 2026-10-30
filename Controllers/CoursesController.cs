using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly QuotesDbContext _context;

        public CoursesController(QuotesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns>List of all courses</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {
            try
            {
                var courses = _context.Courses
                    .OrderBy(c => c.Code)
                    .ToList();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving courses", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific course by ID
        /// </summary>
        /// <param name="id">Course ID</param>
        /// <returns>Course object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Course> GetCourse(int id)
        {
            try
            {
                var course = _context.Courses.Find(id);
                if (course == null)
                {
                    return NotFound(new { error = "Course not found" });
                }
                return Ok(course);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving course", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new course
        /// </summary>
        /// <param name="course">Course object to create</param>
        /// <returns>Created course with ID</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Course> CreateCourse([FromBody] Course course)
        {
            try
            {
                if (course == null)
                {
                    return BadRequest(new { message = "Course object is null" });
                }

                if (string.IsNullOrWhiteSpace(course.Code))
                {
                    return BadRequest(new { message = "Course code is required" });
                }

                if (string.IsNullOrWhiteSpace(course.Name))
                {
                    return BadRequest(new { message = "Course name is required" });
                }

                // Check for duplicate course code
                if (_context.Courses.Any(c => c.Code == course.Code))
                {
                    return BadRequest(new { message = "Course code already exists" });
                }

                course.CreatedAt = DateTime.UtcNow;
                _context.Courses.Add(course);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error creating course", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing course
        /// </summary>
        /// <param name="id">Course ID to update</param>
        /// <param name="course">Updated course object</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateCourse(int id, [FromBody] Course course)
        {
            try
            {
                if (course == null)
                {
                    return BadRequest(new { message = "Course object is null" });
                }

                var entity = _context.Courses.Find(id);
                if (entity == null)
                {
                    return NotFound(new { message = $"Course with ID {id} not found" });
                }

                // Update fields
                if (!string.IsNullOrWhiteSpace(course.Name))
                    entity.Name = course.Name;
                if (!string.IsNullOrWhiteSpace(course.Instructor))
                    entity.Instructor = course.Instructor;
                if (!string.IsNullOrWhiteSpace(course.Semester))
                    entity.Semester = course.Semester;
                if (course.Credits > 0)
                    entity.Credits = course.Credits;

                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error updating course", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="id">Course ID to delete</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCourse(int id)
        {
            try
            {
                var course = _context.Courses.Find(id);
                if (course == null)
                {
                    return NotFound(new { message = $"Course with ID {id} not found" });
                }

                _context.Courses.Remove(course);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error deleting course", error = ex.Message });
            }
        }
    }
}
