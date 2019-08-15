using System.Collections.Generic;
using Zopa.Models;

namespace Zopa.Core.Contracts
{
    /// <summary>
    /// The service containing business logic for the <see cref="Lender"/>.
    /// </summary>
    public interface ILenderService
    {
        /// <summary>
        /// Gets list of <see cref="Lender"/> who are able to lend the sum of the requested amount.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <returns></returns>
        IDictionary<Lender, int> GetLendersWithAmountToLend(int amountRequested);
    }
}
