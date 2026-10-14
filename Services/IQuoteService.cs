using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuotesApi.Models;

namespace QuotesApi.Services
{
    /// <summary>
    /// Interface for Quote repository operations
    /// </summary>
    public interface IQuoteService
    {
        /// <summary>
        /// Get all quotes
        /// </summary>
        IEnumerable<Quote> GetAllQuotes();

        /// <summary>
        /// Get quote by ID
        /// </summary>
        Quote GetQuoteById(int id);

        /// <summary>
        /// Create a new quote
        /// </summary>
        Quote CreateQuote(Quote quote);

        /// <summary>
        /// Update an existing quote
        /// </summary>
        bool UpdateQuote(int id, Quote quote);

        /// <summary>
        /// Delete a quote
        /// </summary>
        bool DeleteQuote(int id);

        /// <summary>
        /// Check if quote exists
        /// </summary>
        bool QuoteExists(int id);
    }
}
