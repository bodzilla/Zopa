using System.Collections.Generic;
using Zopa.Models;

namespace Zopa.Core.Contracts
{
    /// <summary>
    /// The service which calculates the best quotes for a given amount and time.
    /// </summary>
    public interface IQuoteService
    {
        /// <summary>
        /// Gets the best quotes offering the lowest total rates.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <returns></returns>
        IEnumerable<Quote> GetBestQuotes(int amountRequested);
    }
}
