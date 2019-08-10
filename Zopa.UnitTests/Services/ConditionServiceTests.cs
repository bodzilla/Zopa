using System;
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
        public void Setup()
        {
            _logger = new Mock<ILogger<ConditionService>>();
        }

        [OneTimeTearDown]
        public void TearDown() => _conditionService = null;

        [Test]
        public void IsDivisible_UseDivisible_ReturnsTrue()
        {
            #region Arrange

            var settings = new Dictionary<string, string> { { "DivisibleValue", "100" } };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
            _conditionService = new ConditionService(_logger.Object, configuration);

            #endregion

            #region Act

            var result = _conditionService.IsDivisible(200);

            #endregion

            #region Assert

            Assert.That(result, Is.TypeOf<bool>());
            Assert.That(result, Is.True);

            #endregion
        }
    }
}
