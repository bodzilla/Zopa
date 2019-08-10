using System;
using Microsoft.Extensions.Logging;
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
                // Get the best quote out of the available lenders.
                var quote = _quoteService.GetBestQuote(amountRequested);

                if (quote != null)
                {
                    // Interest rate to one decimal point.
                    // Amounts to two decimal points.
                    System.Console.WriteLine($"Requested amount: £{quote.AmountRequested}{Environment.NewLine}" +
                                             $"Annual Interest Rate: {quote.AnnualInterestRatePercentage:0.0}%{Environment.NewLine}" +
                                             $"Monthly repayment: £{quote.MonthlyRepayment:0.00}{Environment.NewLine}" +
                                             $"Total repayment: £{quote.TotalRepayment:0.00}");
                }
                else
                {
                    System.Console.WriteLine("There are no quotes available for your requested amount.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Could not continue application runtime.", ex);
                System.Console.WriteLine("The service could not complete your request.");
            }
        }
    }
}
