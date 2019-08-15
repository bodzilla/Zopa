using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Zopa.Core.Common.Exceptions;
using Zopa.Core.Contracts;
using Zopa.Core.Services;
using Zopa.Models;

namespace Zopa.UnitTests.Services
{
    [TestFixture]
    public class LenderServiceTests
    {
        private ILenderService _lenderService;
        private Mock<ILogger<LenderService>> _logger;
        private Mock<IRepository<Lender>> _repository;

        [OneTimeSetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<LenderService>>();
            _repository = new Mock<IRepository<Lender>>();
        }

        [OneTimeTearDown]
        public void TearDown() => _lenderService = null;

        [Test]
        public void GetLendersWithAmountToLend_FourLenders_ReturnsThreeLenders()
        {
            #region Arrange

            var data = new List<Lender>
            {
                new Lender("Test1",0.01,500),
                new Lender("Test2",0.02,300),
                new Lender("Test3",0.03,200),
                new Lender("Test4",0.04,100)
            };

            var settings = new Dictionary<string, string>
            {
                {
                    "OrderByLowestRate", "True"
                }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
            _repository.Setup(x => x.GetAll()).Returns(data);
            _lenderService = new LenderService(_logger.Object, configuration, _repository.Object);

            #endregion

            #region Act

            var result = _lenderService.GetLendersWithAmountToLend(1000);

            #endregion

            #region Assert

            Assert.That(result, Is.TypeOf<Dictionary<Lender, int>>());
            Assert.That(result.Count, Is.EqualTo(3));

            #endregion
        }

        [Test]
        public void GetLendersWithAmountToLend_NotEnoughLenders_ThrowsAmountRequestedNotRaisedException()
        {
            #region Arrange

            var data = new List<Lender>
            {
                new Lender("Test1",0.01,500)
            };

            var settings = new Dictionary<string, string>
            {
                {
                    "OrderByLowestRate", "True"
                }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
            _repository.Setup(x => x.GetAll()).Returns(data);
            _lenderService = new LenderService(_logger.Object, configuration, _repository.Object);

            #endregion

            #region Act

            #endregion

            #region Assert

            Assert.That(() => _lenderService.GetLendersWithAmountToLend(2000), Throws.TypeOf<AmountRequestedNotRaisedException>());

            #endregion
        }
    }
}
