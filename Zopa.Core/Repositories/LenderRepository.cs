using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Zopa.Core.Contracts;
using Zopa.Models;

namespace Zopa.Core.Repositories
{
    /// <inheritdoc />
    public class LenderRepository : IRepository<Lender>
    {
        private readonly ILogger<LenderRepository> _logger;
        private readonly IDataStore _dataStore;

        public LenderRepository(ILogger<LenderRepository> logger, IDataStore dataStore)
        {
            _logger = logger;
            _dataStore = dataStore;
        }

        /// <inheritdoc />
        public IEnumerable<Lender> GetAll()
        {
            IEnumerable<Lender> lenders;
            try
            {
                lenders = _dataStore.ExtractAllLenders();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get lenders from datastore.", ex);
                throw;
            }
            return lenders;
        }
    }
}
