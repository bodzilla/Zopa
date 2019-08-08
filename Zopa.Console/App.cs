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
                var quote = _quoteService.GetBestQuote(amountRequested, 36);

                if (quote != null)
                {
                    // Interest rate to one decimal point.
                    // Amounts to two decimal points.
                    System.Console.WriteLine($"Requested amount: £{quote.AmountRequested}{Environment.NewLine}" +
                                             $"Annual interest rate: {Math.Round(quote.AnnualInterestRate, 1)}%{Environment.NewLine}" +
                                             $"Monthly repayment: £{Math.Round(quote.MonthlyRepayment, 2)}{Environment.NewLine}" +
                                             $"Total repayment: £{Math.Round(quote.TotalRepayment, 2)}");
                }
                else
                {
                    System.Console.WriteLine("No quotes are available for your requested amount.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Could not continue application runtime.", ex);
                throw;
            }
        }
    }
}
