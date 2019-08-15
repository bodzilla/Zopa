using System.Collections.Generic;
using System.Linq;
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

        [TestCase(36, 100, 0.01, 0.2, 0.3)]
        [TestCase(12, 100, 0.07, 0.09, 0.1)]
        [TestCase(24, 100, 0.01, 0.2, 0.3)]
        public void GetBestQuotes_ThreeLenders_ReturnsBestQuotes(int repaymentLengthMonths, int amountRequested,
            double lowestInterestRate, double higherInterestRateOne, double higherInterestRateTwo)
        {
            #region Arrange

            var settings = new Dictionary<string, string>
            {
                {
                    "RepaymentLengthMonths", repaymentLengthMonths.ToString()
                }
            };

            var lenders = new Dictionary<Lender, int>
            {
                { new Lender("Test1", lowestInterestRate, amountRequested), 100 },
                { new Lender("Test2", higherInterestRateOne, amountRequested), 100 },
                { new Lender("Test3", higherInterestRateTwo, amountRequested), 100 }
            };

            Mock<ILogger<RepaymentService>> repaymentServiceLogger = new Mock<ILogger<RepaymentService>>();
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
            _conditionService.Setup(x => x.IsAmountRequestedValid(It.IsAny<int>())).Returns(true);
            _lenderService.Setup(x => x.GetLendersWithAmountToLend(It.IsAny<int>())).Returns(lenders);
            _quoteService = new QuoteService(_logger.Object, configuration, _lenderService.Object, new RepaymentService(repaymentServiceLogger.Object), _conditionService.Object);

            #endregion

            #region Act

            var result = _quoteService.GetBestQuotes(amountRequested);

            #endregion

            #region Assert

            var list = result.ToList();
            Assert.That(list, Is.TypeOf<List<Quote>>());

            #endregion
        }
    }
}
