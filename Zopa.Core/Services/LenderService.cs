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
        private readonly IRepository<Lender> _repository;

        public LenderService(ILogger<LenderService> logger, IRepository<Lender> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <inheritdoc />
        public IEnumerable<Lender> GetAll()
        {
            try
            {
                var lenders = _repository.GetAll()?.ToList();
                if (lenders != null && lenders.Any())
                {
                    return lenders;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get lenders.", ex);
                throw;
            }
            var exception = new Exception("Lenders collection is empty.");
            _logger.LogError(exception.Message, exception);
            throw exception;
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
