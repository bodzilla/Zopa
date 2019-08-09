using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Zopa.Core.Contracts;
using Zopa.Core.Services;

namespace Zopa.UnitTests.Services
{
    [TestFixture]
    public class RepaymentServiceTests
    {
        private IRepaymentService _repaymentService;
        private Mock<ILogger<RepaymentService>> _logger;

        [OneTimeSetUp]
        public void Setup() => _logger = new Mock<ILogger<RepaymentService>>();

        [OneTimeTearDown]
        public void TearDown() => _repaymentService = null;

        [TestCase(1000, 0.07, 36, 30.877096865371833)]
        [TestCase(10000, 0.07, 36, 308.7709686537184)]
        [TestCase(15000, 0.07, 36, 463.15645298057751)]
        [TestCase(7500, 0.01, 36, 211.56074509672621)]
        [TestCase(2000, 0.055, 36, 60.391803608619966)]
        public void GetMonthlyRepaymentAmount_CalculateAmount_ReturnValidAmount(int amountRequested, double interestRateDecimal,
            int repaymentLengthMonths, double expectedResult)
        {
            #region Arrange

            _repaymentService = new RepaymentService(_logger.Object);

            #endregion

            #region Act

            var result = _repaymentService.GetMonthlyRepaymentAmount(amountRequested, interestRateDecimal, repaymentLengthMonths);

            #endregion

            #region Assert

            Assert.That(result, Is.EqualTo(expectedResult));

            #endregion
        }
    }
}