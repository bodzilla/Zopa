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
        private readonly IEnumerable<Lender> _lenders;

        public LenderRepository(ILogger<LenderRepository> logger, IEnumerable<Lender> lenders)
        {
            _logger = logger;
            _lenders = lenders;
        }

        /// <inheritdoc />
        public IEnumerable<Lender> GetAll()
        {
            try
            {
                return _lenders;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not return lenders", ex);
                throw;
            }
        }
    }
}
