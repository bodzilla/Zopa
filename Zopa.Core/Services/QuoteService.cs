using System;
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
        public Quote GetBestQuote(int amountRequested)
        {
            Quote bestQuote;
            try
            {
                // Check if the amount requested is valid to continue.
                bool amountRequestedValid = _conditionService.IsAmountRequestedValid(amountRequested);
                if (!amountRequestedValid)
                {
                    var exception = new Exception($"{amountRequested} is not within the accepted criteria.");
                    _logger.LogError(exception.Message, exception);
                    throw exception;
                }

                // Generate quotes from lenders who have the required funds.
                var lenders = _lenderService.GetListWithMinAmount(amountRequested);
                var quotes = lenders.Select(lender => CreateQuote(lender, amountRequested)).ToList();

                // The best quote offers the lowest total repayment.
                // Is null when there are no lenders capable of offering a quote.
                bestQuote = quotes.OrderBy(quote => quote.TotalRepayment).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get best quote.", amountRequested, _repaymentLengthMonths, ex);
                throw;
            }
            return bestQuote;
        }

        /// <inheritdoc />
        public Quote CreateQuote(Lender lender, int amountRequested)
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
    }
}
