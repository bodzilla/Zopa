﻿using System;
using Microsoft.Extensions.Logging;
using Zopa.Core.Contracts;

namespace Zopa.Core.Services
{
    /// <inheritdoc />
    public class RepaymentService : IRepaymentService
    {
        private readonly ILogger<RepaymentService> _logger;
        private const int MonthsInYear = 12;

        public RepaymentService(ILogger<RepaymentService> logger) => _logger = logger;

        /// <inheritdoc />
        public double GetMonthlyRepaymentAmount(int amountRequested, double annualInterestRateDecimal, int repaymentLengthMonths)
        {
            double monthlyRepayment;
            try
            {
                // This is the Amortization forumla, adapted from: https://en.wikipedia.org/wiki/Amortization_calculator#The_formula
                var totalPayments = -(MonthsInYear * (repaymentLengthMonths / MonthsInYear));
                var numerator = amountRequested * annualInterestRateDecimal / MonthsInYear;
                var denominator = 1 - Math.Pow(1 + annualInterestRateDecimal / MonthsInYear, totalPayments);
                monthlyRepayment = numerator / denominator;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not calculate monthly repayment rate.", amountRequested, annualInterestRateDecimal, repaymentLengthMonths, ex);
                throw;
            }
            return monthlyRepayment;
        }
    }
}
