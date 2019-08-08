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

        public QuoteService(ILogger<QuoteService> logger, ILenderService lenderService, IRepaymentService repaymentService)
        {
            _logger = logger;
            _lenderService = lenderService;
            _repaymentService = repaymentService;
        }

        /// <inheritdoc />
        public Quote GetBestQuote(int amountRequested, int repaymentLengthMonths)
        {
            Quote bestQuote;
            try
            {
                var lenders = _lenderService.GetListWithMinAmount(amountRequested);
                var quotes = GetQuotes(lenders, amountRequested, repaymentLengthMonths);
                bestQuote = quotes.OrderBy(quote => quote.TotalRepayment).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get quotes.", amountRequested, repaymentLengthMonths, ex);
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
