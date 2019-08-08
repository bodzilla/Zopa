using System;
using Microsoft.Extensions.Logging;
using Zopa.Core.Contracts;

namespace Zopa.Core.Common
{
    /// <inheritdoc />
    public class MonthlyRepaymentCalculator : IRepaymentCalculator
    {
        private readonly ILogger<MonthlyRepaymentCalculator> _logger;
        private const int MonthsInYear = 12;

        public MonthlyRepaymentCalculator(ILogger<MonthlyRepaymentCalculator> logger) => _logger = logger;

        /// <inheritdoc />
        /// Uses Amortization formula found in: https://en.wikipedia.org/wiki/Amortization_calculator#The_formula
        public double CalculateRepayment(int amountRequested, double interestRateDecimal, int repaymentLengthMonths)
        {
            double monthlyRepayment;
            try
            {
                var totalPayments = -(MonthsInYear * (repaymentLengthMonths / MonthsInYear));
                var numerator = amountRequested * interestRateDecimal / MonthsInYear;
                var denominator = 1 - Math.Pow(1 + interestRateDecimal / MonthsInYear, totalPayments);
                monthlyRepayment = numerator / denominator;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not calculate monthly repayment rate.", amountRequested, interestRateDecimal, repaymentLengthMonths, ex);
                throw;
            }
            return monthlyRepayment;
        }
    }
}
