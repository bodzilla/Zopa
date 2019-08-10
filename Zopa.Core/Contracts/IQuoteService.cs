using Zopa.Models;

namespace Zopa.Core.Contracts
{
    /// <summary>
    /// The service which calculates the best quotes for a given amount and time.
    /// </summary>
    public interface IQuoteService
    {
        /// <summary>
        /// Gets the best quote offers the lowest total repayment. Returns null when there are no lenders capable of offering a quote.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <returns></returns>
        Quote GetBestQuote(int amountRequested);

        /// <summary>
        /// Creates <see cref="Quote"/> for the given amount and time.
        /// </summary>
        /// <param name="lender"></param>
        /// <param name="amountRequested"></param>
        /// <returns></returns>
        Quote CreateQuote(Lender lender, int amountRequested);
    }
}
