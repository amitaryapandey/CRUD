using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private QuotesDbContext _quotesDbContext;
        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        /// <summary>
        /// Get all quotes
        /// </summary>
        /// <returns>List of all quotes</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Quote>> Get()
        {
            try
            {
                var quotes = _quotesDbContext.Quotes.ToList();
                return Ok(quotes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error retrieving quotes", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific quote by ID
        /// </summary>
        /// <param name="id">Quote ID</param>
        /// <returns>Quote object</returns>
        [HttpGet("{id}", Name = "GetQuote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Quote> Get(int id)
        {
            try
            {
                var quote = _quotesDbContext.Quotes.Find(id);
                if (quote == null)
                {
                    return NotFound(new { message = $"Quote with ID {id} not found" });
                }
                return Ok(quote);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error retrieving quote", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new quote
        /// </summary>
        /// <param name="quote">Quote object to create</param>
        /// <returns>Created quote with ID</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Quote> Post([FromBody] Quote quote)
        {
            try
            {
                if (quote == null)
                {
                    return BadRequest(new { message = "Quote object is null" });
                }

                if (string.IsNullOrWhiteSpace(quote.Title))
                {
                    return BadRequest(new { message = "Title is required" });
                }

                _quotesDbContext.Quotes.Add(quote);
                _quotesDbContext.SaveChanges();

                return CreatedAtRoute("GetQuote", new { id = quote.Id }, quote);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error creating quote", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing quote
        /// </summary>
        /// <param name="id">Quote ID to update</param>
        /// <param name="quote">Updated quote object</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Put(int id, [FromBody] Quote quote)
        {
            try
            {
                if (quote == null)
                {
                    return BadRequest(new { message = "Quote object is null" });
                }

                var entity = _quotesDbContext.Quotes.Find(id);
                if (entity == null)
                {
                    return NotFound(new { message = $"Quote with ID {id} not found" });
                }

                entity.Title = quote.Title ?? entity.Title;
                entity.Author = quote.Author ?? entity.Author;
                entity.Description = quote.Description ?? entity.Description;

                _quotesDbContext.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error updating quote", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a quote
        /// </summary>
        /// <param name="id">Quote ID to delete</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                var quote = _quotesDbContext.Quotes.Find(id);
                if (quote == null)
                {
                    return NotFound(new { message = $"Quote with ID {id} not found" });
                }

                _quotesDbContext.Quotes.Remove(quote);
                _quotesDbContext.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error deleting quote", error = ex.Message });
            }
        }
    }
}
