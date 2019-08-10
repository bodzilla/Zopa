using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
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
        public void GetAll_ThreeLenders_ReturnsThreeLenders()
        {
            #region Arrange

            var data = new List<Lender>
            {
                new Lender("Test1",1,1),
                new Lender("Test2",2,10),
                new Lender("Test3",3,100),
            };

            _repository.Setup(x => x.GetAll()).Returns(data);
            _lenderService = new LenderService(_logger.Object, _repository.Object);

            #endregion

            #region Act

            var result = _lenderService.GetAll();

            #endregion

            #region Assert

            var list = result.ToList();
            Assert.That(list, Is.TypeOf<List<Lender>>());
            Assert.That(list.Count, Is.EqualTo(3));

            #endregion
        }

        [Test]
        public void GetAll_EmptyCollection_ThrowsException()
        {
            #region Arrange

            _repository.Setup(x => x.GetAll()).Returns(new List<Lender>());
            _lenderService = new LenderService(_logger.Object, _repository.Object);

            #endregion

            #region Act And Assert

            Assert.That(() => _lenderService.GetAll(), Throws.Exception);

            #endregion
        }

        [Test]
        public void GetAll_NullResult_ThrowsException()
        {
            #region Arrange

            _repository.Setup(x => x.GetAll()).Returns(() => null);
            _lenderService = new LenderService(_logger.Object, _repository.Object);

            #endregion

            #region Act And Assert

            Assert.That(() => _lenderService.GetAll(), Throws.Exception);

            #endregion
        }

        [Test]
        public void GetListWithMinAmount_FiveAvailableLenders_ReturnsThreeValidLenders()
        {
            #region Arrange

            var data = new List<Lender>
            {
                new Lender("Test1",1,100),
                new Lender("Test2",2,200),
                new Lender("Test3",3,1000),
                new Lender("Test4",4,2500),
                new Lender("Test5",5,10000)
            };

            _repository.Setup(x => x.GetAll()).Returns(data);
            _lenderService = new LenderService(_logger.Object, _repository.Object);

            #endregion

            #region Act

            var result = _lenderService.GetListWithMinAmount(1000);

            #endregion

            #region Assert

            var list = result.ToList();
            Assert.That(list, Is.TypeOf<List<Lender>>());
            Assert.That(list.Count, Is.EqualTo(3));

            #endregion
        }

        [Test]
        public void GetListWithMinAmount_EmptyCollection_ThrowsException()
        {
            #region Arrange

            _repository.Setup(x => x.GetAll()).Returns(new List<Lender>());
            _lenderService = new LenderService(_logger.Object, _repository.Object);

            #endregion

            #region Act And Assert

            Assert.That(() => _lenderService.GetListWithMinAmount(1000), Throws.Exception);

            #endregion
        }

        [Test]
        public void GetListWithMinAmount_NullResult_ThrowsException()
        {
            #region Arrange

            _repository.Setup(x => x.GetAll()).Returns(() => null);
            _lenderService = new LenderService(_logger.Object, _repository.Object);

            #endregion

            #region Act And Assert

            Assert.That(() => _lenderService.GetListWithMinAmount(1000), Throws.Exception);

            #endregion
        }
    }
}
