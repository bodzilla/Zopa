using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Zopa.Core.Contracts;
using Zopa.Models;

namespace Zopa.Core.Services
{
    /// <inheritdoc />
    public class LenderService : ILenderService
    {
        private readonly ILogger<LenderService> _logger;
        private readonly IDataStore _dataStore;

        public LenderService(ILogger<LenderService> logger, IDataStore dataStore)
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
                lenders = _dataStore.ExtractAll();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get lenders.", ex);
                throw;
            }
            return lenders;
        }

        /// <inheritdoc />
        public IEnumerable<Lender> GetListWithMinAmount(int minimumAmount)
        {
            IEnumerable<Lender> lenders;
            try
            {
                lenders = GetAll().Where(lender => lender.CashAvailable >= minimumAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get lenders with minimum amount.", minimumAmount, ex);
                throw;
            }
            return lenders;
        }
    }
}
