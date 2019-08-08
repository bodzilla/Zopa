using System;
using System.Collections.Generic;
using System.Linq;
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

        public QuoteService(ILogger<QuoteService> logger, ILenderService lenderService, IRepaymentService repaymentService, IConditionService conditionService)
        {
            _logger = logger;
            _lenderService = lenderService;
            _repaymentService = repaymentService;
            _conditionService = conditionService;
        }

        /// <inheritdoc />
        public Quote GetBestQuote(int amountRequested, int repaymentLengthMonths)
        {
            Quote bestQuote;
            try
            {
                // Check if the amount requested is valid to continue.
                var amountRequestedValid = _conditionService.CheckAmountRequestedValid(amountRequested);
                if (!amountRequestedValid)
                {
                    var ex = new Exception($"£{amountRequested} is not within the accepted criteria.");
                    _logger.LogError(ex.Message, ex);
                    throw ex;
                }

                // Get all lenders and assess which of them have the funds to offer a quote.
                var lenders = _lenderService.GetListWithMinAmount(amountRequested);
                var quotes = GetQuotes(lenders, amountRequested, repaymentLengthMonths);

                // The best quote offers the lowest total repayment.
                bestQuote = quotes.OrderBy(quote => quote.TotalRepayment).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get best quote.", amountRequested, repaymentLengthMonths, ex);
                throw;
            }
            return bestQuote;
        }

        /// <inheritdoc />
        public IEnumerable<Quote> GetQuotes(IEnumerable<Lender> lenders, int amountRequested, int repaymentLengthMonths)
        {
            var quotes = new List<Quote>();
            try
            {
                foreach (var lender in lenders)
                {
                    // Calculate the monthly repayment for current lender and add it to the quote list.
                    var monthlyRepayment = _repaymentService.GetMonthlyRepaymentAmount(amountRequested, lender.InterestRateDecimal, repaymentLengthMonths);
                    var quote = new Quote(amountRequested, lender.InterestRateDecimal, monthlyRepayment, repaymentLengthMonths);
                    quotes.Add(quote);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get quotes.", lenders, amountRequested, repaymentLengthMonths, ex);
                throw;
            }
            return quotes;
        }
    }
}
