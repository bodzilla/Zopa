using Zopa.Models;

namespace Zopa.Core.Contracts
{
    /// <summary>
    /// The service which calculates the best quotes for a given amount and time.
    /// </summary>
    public interface IQuoteService
    {
        /// <summary>
        /// Calculates the best quote for the given amount and time.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <param name="repaymentLengthMonths"></param>
        /// <returns></returns>
        Quote GetBestQuote(int amountRequested, int repaymentLengthMonths);
    }
}
