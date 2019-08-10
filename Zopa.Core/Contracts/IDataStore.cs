using System.Collections.Generic;
using Zopa.Models;

namespace Zopa.Core.Contracts
{
    public interface IDataStore
    {
        /// <summary>
        /// Extracts all <see cref="Lender"/> from CSV.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lender> ExtractAllLenders();
    }
}
