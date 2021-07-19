using AspNetCoreDapper.Models;
using AspNetCoreDapper.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCoreDapperTests.Repositories
{
    [TestClass]
    public class FleetRepositoryTests : Base
    {
        private FleetRepository _repository;

        public FleetRepositoryTests()
        {
            _repository = new FleetRepository(_config);
        }

        [TestMethod]
        public void Add_Test()
        {
            var entity = new Fleet
            {
                Code = "frota10",
                Name = "Frota 10",
                IsEnabled = true,
                Current = false
            };

            _repository.Add(entity);

            Assert.IsNotNull(true);
            Assert.IsTrue(true);
        }
    }
}
