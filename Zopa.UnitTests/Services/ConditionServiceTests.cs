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

        #region Is Divisible And Within Range

        [TestCase("100", "100", "500", 200, true)]
        [TestCase("100", "-500", "-100", -200, true)]
        [TestCase("100", "100", "500", 100, true)]
        [TestCase("100", "100", "500", 500, true)]

        #endregion
        #region Is Divisible And Not Within Range

        [TestCase("99", "100", "500", 99, false)]
        [TestCase("501", "100", "500", 501, false)]
        [TestCase("100", "100", "500", -100, false)]

        #endregion
        #region Is Not Divisible And Within Range

        [TestCase("2", "100", "200", 101, false)]
        [TestCase("2", "-200", "-100", -101, false)]

        #endregion
        #region Is Not Divisible And Not Within Range
        [TestCase("2", "100", "200", 201, false)]
        [TestCase("2", "-200", "-100", -99, false)]
        [TestCase("2", "-200", "-100", -201, false)]
        #endregion
        public void IsAmountRequestedValid_UseValues_ReturnsExpectedResult(string divisibleValue, string acceptanceRangeBottom, string acceptanceRangeTop, int amountRequested, bool expectedResult)
        {
            #region Arrange

            var settings = new Dictionary<string, string>
            {
                {
                    "DivisibleValue", divisibleValue
                },
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

            var result = _conditionService.IsAmountRequestedValid(amountRequested);

            #endregion

            #region Assert

            Assert.That(result, Is.TypeOf<bool>());
            Assert.That(result, Is.EqualTo(expectedResult));

            #endregion
        }
    }
}
