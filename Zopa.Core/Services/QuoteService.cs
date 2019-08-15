using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Zopa.Core.Contracts;
using Zopa.Models;

namespace Zopa.Core.Services
{
    /// <inheritdoc />
    public class QuoteService : IQuoteService
    {
        private readonly ILogger<QuoteService> _logger;
        private readonly ILenderService _lenderService;
        private readonly IRepaymentService _repaymentService;
        private readonly IConditionService _conditionService;
        private readonly int _repaymentLengthMonths;

        public QuoteService(ILogger<QuoteService> logger, IConfiguration configuration,
            ILenderService lenderService, IRepaymentService repaymentService, IConditionService conditionService)
        {
            _logger = logger;
            _lenderService = lenderService;
            _repaymentService = repaymentService;
            _conditionService = conditionService;
            _repaymentLengthMonths = configuration.GetValue<int>("RepaymentLengthMonths");
        }

        /// <inheritdoc />
        public IEnumerable<Quote> GetBestQuotes(int amountRequested)
        {
            IList<Quote> quotes;
            try
            {
                CheckRequestedAmountValid(amountRequested);
                var lenders = _lenderService.GetLendersWithAmountToLend(amountRequested);
                quotes = lenders.Select(lender => CreateQuote(lender.Key, lender.Value)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get best quotes.", amountRequested, ex);
                throw;
            }
            return quotes;
        }

        /// <summary>
        /// Creates <see cref="Quote"/> for the given amount and time.
        /// </summary>
        /// <param name="lender"></param>
        /// <param name="amountRequested"></param>
        /// <returns></returns>
        private Quote CreateQuote(Lender lender, int amountRequested)
        {
            Quote quote;
            try
            {
                // Calculate the monthly repayment using the lender's interest rate and repayment length, then create quote.
                var monthlyRepayment = _repaymentService.GetMonthlyRepaymentAmount(amountRequested, lender.AnnualInterestRateDecimal, _repaymentLengthMonths);
                quote = new Quote(amountRequested, lender.AnnualInterestRateDecimal, monthlyRepayment, _repaymentLengthMonths);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get quote.", lender, amountRequested, _repaymentLengthMonths, ex);
                throw;
            }
            return quote;
        }

        /// <summary>
        /// Checks if the amount requested is valid.
        /// </summary>
        /// <param name="amountRequested"></param>
        private void CheckRequestedAmountValid(int amountRequested)
        {
            bool amountRequestedValid = _conditionService.IsAmountRequestedValid(amountRequested);
            if (amountRequestedValid)
            {
                return;
            }
            var exception = new Exception($"{amountRequested} is not within the accepted criteria.");
            _logger.LogError(exception.Message, exception);
            throw exception;
        }
    }
}
