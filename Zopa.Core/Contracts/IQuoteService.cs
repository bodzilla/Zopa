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
        /// Calculates the best <see cref="Quote"/> for the given amount and time.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <returns></returns>
        Quote GetBestQuote(int amountRequested);

        /// <summary>
        /// Get all <see cref="Quote"/> for the given amount and time.
        /// </summary>
        /// <param name="lenders"></param>
        /// <param name="amountRequested"></param>
        /// <returns></returns>
        IEnumerable<Quote> GetQuotes(IEnumerable<Lender> lenders, int amountRequested);
    }
}
