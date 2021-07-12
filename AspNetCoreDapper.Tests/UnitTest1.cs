using AspNetCoreDapper.Models;
using AspNetCoreDapper.Controllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCoreDapper.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public FleetController _fleetController { get; set; }
        public VehicleController _vehicleController { get; set; }

        [TestMethod]
        public void InsertFleet()
        {
            var entity = new Fleet()
            {
                Id = 0,
                Code = "frota10",
                Name = "Frota 10"
            };

            var result = _fleetController.Create(entity);

            Assert.IsNotNull(result);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InsertVehicle()
        {
            var entity = new Vehicle()
            {
                Id = 0,
                Chassi = "1488dfsfds",
                Type = "bus",
                NumberPassengers = 42,
                Color = "Verde"
            };

            var result = _vehicleController.Create(entity);

            Assert.IsNotNull(result);
            Assert.IsTrue(true);
        }
    }
}
