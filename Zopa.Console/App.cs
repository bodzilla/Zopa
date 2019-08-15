using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Zopa.Core.Common.Exceptions;
using Zopa.Core.Contracts;

namespace Zopa.Console
{
    public sealed class App
    {
        private readonly ILogger<App> _logger;
        private readonly IQuoteService _quoteService;

        public App(ILogger<App> logger, IQuoteService quoteService)
        {
            _logger = logger;
            _quoteService = quoteService;
        }

        public void Run(int amountRequested)
        {
            try
            {
                var quotes = _quoteService.GetBestQuotes(amountRequested).ToList();

                foreach (var quote in quotes)
                {
                    System.Console.WriteLine($"Requested amount: £{quote.AmountRequested}{Environment.NewLine}" +
                                             $"Annual Interest Rate: {quote.AnnualInterestRatePercentage:0.0}%{Environment.NewLine}" +
                                             $"Monthly repayment: £{quote.MonthlyRepayment:0.00}{Environment.NewLine}" +
                                             $"Total repayment: £{quote.TotalRepayment:0.00}{Environment.NewLine}");
                }
            }
            catch (AmountRequestedNotRaisedException)
            {
                System.Console.WriteLine("The service could not provide enough quotes for your requested amount.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Could not continue application runtime.", ex);
                System.Console.WriteLine("The service could not complete your request.");
            }
        }
    }
}
