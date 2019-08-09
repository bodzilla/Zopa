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
        public Quote GetBestQuote(int amountRequested)
        {
            Quote bestQuote;
            try
            {
                // Check if the amount requested is valid to continue.
                bool amountRequestedValid = _conditionService.CheckAmountRequestedValid(amountRequested);
                if (!amountRequestedValid)
                {
                    var exception = new Exception($"£{amountRequested} is not within the accepted criteria.");
                    _logger.LogError(exception.Message, exception);
                    throw exception;
                }

                // Get all lenders and assess which of them have the funds to offer a quote.
                var lenders = _lenderService.GetListWithMinAmount(amountRequested);
                var quotes = GetQuotes(lenders, amountRequested);

                // The best quote offers the lowest total repayment or null.
                bestQuote = quotes?.OrderBy(quote => quote.TotalRepayment).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get best quote.", amountRequested, _repaymentLengthMonths, ex);
                throw;
            }
            return bestQuote;
        }

        /// <inheritdoc />
        public IEnumerable<Quote> GetQuotes(IEnumerable<Lender> lenders, int amountRequested)
        {
            var quotes = new List<Quote>();
            try
            {
                foreach (var lender in lenders)
                {
                    // Calculate the monthly repayment for current lender and add it to the quote list.
                    var monthlyRepayment = _repaymentService.GetMonthlyRepaymentAmount(amountRequested, lender.InterestRateDecimal, _repaymentLengthMonths);
                    var quote = new Quote(amountRequested, lender.InterestRateDecimal, monthlyRepayment, _repaymentLengthMonths);
                    quotes.Add(quote);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get quotes.", lenders, amountRequested, _repaymentLengthMonths, ex);
                throw;
            }
            return quotes;
        }
    }
}
