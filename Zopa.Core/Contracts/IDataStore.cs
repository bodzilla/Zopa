using System.Collections.Generic;
using Zopa.Models;

namespace Zopa.Core.Contracts
{
    /// <summary>
    /// The interface from which all data extractors inherit from. 
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// Extracts all <see cref="Lender"/> from data source.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lender> ExtractAll();
    }
}
