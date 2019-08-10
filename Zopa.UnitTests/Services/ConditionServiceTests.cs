using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Zopa.Core.Contracts;
using Zopa.Core.Services;

namespace Zopa.UnitTests.Services
{
    [TestFixture]
    public class ConditionServiceTests
    {
        private IConditionService _conditionService;
        private Mock<ILogger<ConditionService>> _logger;

        [OneTimeSetUp]
        public void Setup() => _logger = new Mock<ILogger<ConditionService>>();

        [OneTimeTearDown]
        public void TearDown() => _conditionService = null;

        [TestCase("100", 200, true)]
        [TestCase("10", 20, true)]
        [TestCase("1", 2, true)]
        [TestCase("100", 201, false)]
        [TestCase("10", 21, false)]
        [TestCase("5", 1, false)]
        public void IsDivisible_UseDivisible_ReturnsExpectedResult(string divisibleValue, int amountRequested, bool expectedResult)
        {
            #region Arrange

            var settings = new Dictionary<string, string>
            {
                {
                    "DivisibleValue", divisibleValue
                }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
            _conditionService = new ConditionService(_logger.Object, configuration);

            #endregion

            #region Act

            var result = _conditionService.IsDivisible(amountRequested);

            #endregion

            #region Assert

            Assert.That(result, Is.TypeOf<bool>());
            Assert.That(result, Is.EqualTo(expectedResult));

            #endregion
        }

        [TestCase("100", "500", 200, true)]
        [TestCase("-500", "-100", -200, true)]
        [TestCase("100", "500", 100, true)]
        [TestCase("100", "500", 500, true)]
        [TestCase("100", "500", 99, false)]
        [TestCase("100", "500", 501, false)]
        [TestCase("100", "500", -100, false)]
        public void IsWithinRange_UseAcceptanceRanges_ReturnsExpectedResult(string acceptanceRangeBottom, string acceptanceRangeTop, int amountRequested, bool expectedResult)
        {
            #region Arrange

            var settings = new Dictionary<string, string>
            {
                {
                    "AcceptanceRangeBottom", acceptanceRangeBottom
                },
                {
                    "AcceptanceRangeTop", acceptanceRangeTop
                }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
            _conditionService = new ConditionService(_logger.Object, configuration);

            #endregion

            #region Act

            var result = _conditionService.IsWithinRange(amountRequested);

            #endregion

            #region Assert

            Assert.That(result, Is.TypeOf<bool>());
            Assert.That(result, Is.EqualTo(expectedResult));

            #endregion
        }
    }
}
