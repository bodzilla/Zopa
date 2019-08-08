using System;
using Microsoft.Extensions.Logging;
using Zopa.Core.Contracts;

namespace Zopa.Core.Services
{
    /// <inheritdoc />
    public class RepaymentService : IRepaymentService
    {
        private readonly ILogger<RepaymentService> _logger;
        private readonly IRepaymentCalculator _calculator;

        public RepaymentService(ILogger<RepaymentService> logger, IRepaymentCalculator calculator)
        {
            _logger = logger;
            _calculator = calculator;
        }

        /// <inheritdoc />
        public double GetRepaymentAmount(int amountRequested, double interestRateDecimal, int repaymentLengthMonths)
        {
            double monthlyRepayment;
            try
            {
                monthlyRepayment = _calculator.CalculateRepayment(amountRequested, interestRateDecimal, repaymentLengthMonths);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get repayment amount.", ex);
                throw;
            }
            return monthlyRepayment;
        }
    }
}
