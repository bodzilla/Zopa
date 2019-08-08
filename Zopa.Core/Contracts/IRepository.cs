using System.Collections.Generic;

namespace Zopa.Core.Contracts
{
    /// <summary>
    /// The interface from which all data extractors inherit from. 
    /// </summary>
    public interface IRepository<out T>
    {
        /// <summary>
        /// Get all <see cref="T"/> from data source.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
    }
}
