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
        /// Gets list of all <see cref="Lender"/> from data source.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lender> GetAll();

        /// <summary>
        /// Gets list of <see cref="Lender"/> who have atleast the minimum amount requested.
        /// </summary>
        /// <param name="minimumAmount"></param>
        /// <returns></returns>
        IEnumerable<Lender> GetListWithMinAmount(int minimumAmount);
    }
}
