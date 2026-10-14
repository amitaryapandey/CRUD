using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuotesApi.Data;
using QuotesApi.Models;

namespace QuotesApi.Services
{
    /// <summary>
    /// Quote service implementation for CRUD operations
    /// </summary>
    public class QuoteService : IQuoteService
    {
        private readonly QuotesDbContext _dbContext;

        public QuoteService(QuotesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all quotes from database
        /// </summary>
        public IEnumerable<Quote> GetAllQuotes()
        {
            return _dbContext.Quotes.ToList();
        }

        /// <summary>
        /// Get a specific quote by ID
        /// </summary>
        public Quote GetQuoteById(int id)
        {
            return _dbContext.Quotes.Find(id);
        }

        /// <summary>
        /// Create a new quote
        /// </summary>
        public Quote CreateQuote(Quote quote)
        {
            if (quote == null)
                throw new ArgumentNullException(nameof(quote));

            if (string.IsNullOrWhiteSpace(quote.Title))
                throw new ArgumentException("Title is required", nameof(quote.Title));

            _dbContext.Quotes.Add(quote);
            _dbContext.SaveChanges();
            return quote;
        }

        /// <summary>
        /// Update an existing quote
        /// </summary>
        public bool UpdateQuote(int id, Quote quote)
        {
            if (quote == null)
                throw new ArgumentNullException(nameof(quote));

            var entity = _dbContext.Quotes.Find(id);
            if (entity == null)
                return false;

            entity.Title = quote.Title ?? entity.Title;
            entity.Author = quote.Author ?? entity.Author;
            entity.Description = quote.Description ?? entity.Description;

            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Delete a quote
        /// </summary>
        public bool DeleteQuote(int id)
        {
            var quote = _dbContext.Quotes.Find(id);
            if (quote == null)
                return false;

            _dbContext.Quotes.Remove(quote);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Check if a quote exists
        /// </summary>
        public bool QuoteExists(int id)
        {
            return _dbContext.Quotes.Any(q => q.Id == id);
        }
    }
}
