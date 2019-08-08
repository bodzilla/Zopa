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
        private readonly IConditions _conditions;

        public QuoteService(ILogger<QuoteService> logger, ILenderService lenderService, IRepaymentService repaymentService, IConditions conditions)
        {
            _logger = logger;
            _lenderService = lenderService;
            _repaymentService = repaymentService;
            _conditions = conditions;
        }

        /// <inheritdoc />
        public Quote GetBestQuote(int amountRequested, int repaymentLengthMonths)
        {
            Quote bestQuote;
            try
            {
                var amountRequestedValid = _conditions.CheckAmountRequestedValid(amountRequested);
                if (!amountRequestedValid)
                {
                    var ex = new Exception($"£{amountRequested} is not within the accepted criteria.");
                    _logger.LogError(ex.Message, ex);
                    throw ex;
                }
                var lenders = _lenderService.GetListWithMinAmount(amountRequested);
                var quotes = GetQuotes(lenders, amountRequested, repaymentLengthMonths);
                bestQuote = quotes.OrderBy(quote => quote.TotalRepayment).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get best quote.", amountRequested, repaymentLengthMonths, ex);
                throw;
            }
            return bestQuote;
        }

        public IEnumerable<Quote> GetQuotes(IEnumerable<Lender> lenders, int amountRequested, int repaymentLengthMonths)
        {
            var quotes = new List<Quote>();
            try
            {
                foreach (var lender in lenders)
                {
                    var incrementalRepaymentAmount = _repaymentService.GetRepaymentAmount(amountRequested, lender.InterestRateDecimal, repaymentLengthMonths);
                    var quote = new Quote(amountRequested, lender.InterestRateDecimal, incrementalRepaymentAmount, repaymentLengthMonths);
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
