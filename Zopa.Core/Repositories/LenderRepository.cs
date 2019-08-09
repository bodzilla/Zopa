using System;
using System.Collections.Generic;
using System.Linq;
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
            if (_lenders != null && _lenders.Any())
            {
                return _lenders;
            }
            var ex = new Exception("Lenders collection is empty or null.");
            _logger.LogError(ex.Message, ex);
            throw ex;
        }
    }
}
