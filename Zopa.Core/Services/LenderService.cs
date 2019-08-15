using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Zopa.Core.Common.Exceptions;
using Zopa.Core.Contracts;
using Zopa.Models;

namespace Zopa.Core.Services
{
    /// <inheritdoc />
    public class LenderService : ILenderService
    {
        private readonly ILogger<LenderService> _logger;
        private readonly IRepository<Lender> _repository;
        private readonly bool _orderByLowestRate;

        public LenderService(ILogger<LenderService> logger, IConfiguration configuration, IRepository<Lender> repository)
        {
            _logger = logger;
            _repository = repository;
            _orderByLowestRate = configuration.GetValue<bool>("OrderByLowestRate");
        }

        /// <inheritdoc />
        public IDictionary<Lender, int> GetLendersWithAmountToLend(int amountRequested)
        {
            var lenders = new Dictionary<Lender, int>();
            try
            {
                var availableLenders = GetOrderedLenders();
                int amountRaised = 0;

                while (amountRaised < amountRequested)
                {
                    if (availableLenders.Count < 1)
                    {
                        throw new AmountRequestedNotRaisedException();
                    }

                    var lender = availableLenders.Pop();
                    var totalWithLendersCash = amountRaised + lender.CashAvailable;

                    // Calculate if all this lender's cash will be required or just a part.
                    if (totalWithLendersCash > amountRequested)
                    {
                        var leftToRaise = amountRequested - amountRaised;
                        lenders.Add(lender, leftToRaise);
                        amountRaised += leftToRaise;
                    }
                    else
                    {
                        lenders.Add(lender, lender.CashAvailable);
                        amountRaised += lender.CashAvailable;
                    }
                }
            }
            catch (AmountRequestedNotRaisedException ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get ordered lenders.", ex);
                throw;
            }
            return lenders;
        }

        /// <summary>
        /// Get ordered list of <see cref="Lender"/>.
        /// </summary>
        /// <returns></returns>
        private Stack<Lender> GetOrderedLenders()
        {
            try
            {
                var lenders = GetAll().ToList();
                var orderedLenders = _orderByLowestRate ?
                    new Stack<Lender>(lenders.OrderByDescending(x => x.AnnualInterestRateDecimal)) :
                    new Stack<Lender>(lenders.OrderBy(x => x.AnnualInterestRateDecimal));
                return orderedLenders;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get ordered lenders.", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets list of all <see cref="Lender"/> from data source.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Lender> GetAll()
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
    }
}
