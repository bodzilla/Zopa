using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Zopa.Core.Contracts;
using Zopa.Core.Repositories;
using Zopa.Models;

namespace Zopa.UnitTests.Repositories
{
    [TestFixture]
    public class LenderRepositoryTests
    {
        private IRepository<Lender> _lenderRepository;
        private Mock<ILogger<LenderRepository>> _logger;
        private Mock<IDataStore> _dataStore;

        [OneTimeSetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<LenderRepository>>();
            _dataStore = new Mock<IDataStore>();
        }

        [OneTimeTearDown]
        public void TearDown() => _lenderRepository = null;

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

            _dataStore.Setup(x => x.ExtractAllLenders()).Returns(data);
            _lenderRepository = new LenderRepository(_logger.Object, _dataStore.Object);

            #endregion

            #region Act

            var result = _lenderRepository.GetAll();

            #endregion

            #region Assert

            var list = result.ToList();
            Assert.That(list, Is.TypeOf<List<Lender>>());
            Assert.That(list.Count, Is.EqualTo(3));

            #endregion
        }

        [Test]
        public void GetAll_EmptyCollection_ReturnsNoLenders()
        {
            #region Arrange

            _dataStore.Setup(x => x.ExtractAllLenders()).Returns(new List<Lender>());
            _lenderRepository = new LenderRepository(_logger.Object, _dataStore.Object);

            #endregion

            #region Act

            var result = _lenderRepository.GetAll();

            #endregion

            #region Assert

            var list = result.ToList();
            Assert.That(list, Is.TypeOf<List<Lender>>());
            Assert.That(list.Count, Is.EqualTo(0));

            #endregion
        }

        [Test]
        public void GetAll_NullResult_ReturnsNull()
        {
            #region Arrange

            _dataStore.Setup(x => x.ExtractAllLenders()).Returns(() => null);
            _lenderRepository = new LenderRepository(_logger.Object, _dataStore.Object);

            #endregion

            #region Act

            var result = _lenderRepository.GetAll();

            #endregion

            #region Assert

            Assert.That(result, Is.Null);

            #endregion
        }
    }
}
