using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Zopa.Core.Contracts;
using Zopa.Core.Services;
using Zopa.Models;

namespace Zopa.UnitTests.Services
{
    [TestFixture]
    public class QuoteServiceTests
    {
        private IQuoteService _quoteService;
        private Mock<ILogger<QuoteService>> _logger;
        private Mock<ILenderService> _lenderService;
        private Mock<IRepaymentService> _repaymentService;
        private Mock<IConditionService> _conditionService;

        [OneTimeSetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<QuoteService>>();
            _lenderService = new Mock<ILenderService>();
            _repaymentService = new Mock<IRepaymentService>();
            _conditionService = new Mock<IConditionService>();
        }

        [OneTimeTearDown]
        public void TearDown() => _conditionService = null;

        [TestCase(36, 10, 1, 100, 1000)]
        [TestCase(36, 100, 1, 1000, 1000)]
        [TestCase(12, 100, 1, 1000, 1000)]
        [TestCase(24, 100, 1, 1000, 1000)]
        public void GetQuote_FulfilQuote_ReturnsValidQuote(int repaymentLengthMonths, int monthlyRepaymentAmount, int annualInterestRateDecimal, int cashAvailable, int amountRequested)
        {
            #region Arrange

            _repaymentService
                .Setup(x => x.GetMonthlyRepaymentAmount(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<int>()))
                .Returns(monthlyRepaymentAmount);

            var settings = new Dictionary<string, string>
            {
                {
                    "RepaymentLengthMonths", repaymentLengthMonths.ToString()
                }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

            _quoteService = new QuoteService(_logger.Object, configuration, _lenderService.Object, _repaymentService.Object, _conditionService.Object);

            #endregion

            #region Act

            var result = _quoteService.CreateQuote(new Lender(String.Empty, annualInterestRateDecimal, cashAvailable), amountRequested);

            #endregion

            #region Assert

            Assert.That(result, Is.TypeOf<Quote>());
            Assert.That(result.AmountRequested, Is.EqualTo(amountRequested));
            Assert.That(result.AnnualInterestRateDecimal, Is.EqualTo(annualInterestRateDecimal));
            Assert.That(result.AnnualInterestRatePercentage, Is.EqualTo(annualInterestRateDecimal * 100));
            Assert.That(result.MonthlyRepayment, Is.EqualTo(monthlyRepaymentAmount));
            Assert.That(result.TotalRepayment, Is.EqualTo(monthlyRepaymentAmount * repaymentLengthMonths));

            #endregion
        }
    }
}
