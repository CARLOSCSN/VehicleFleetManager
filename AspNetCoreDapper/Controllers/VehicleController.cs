using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreDapper.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreDapper.Repositories;

namespace AspNetCoreDapper.Controllers
{
    public class VehicleController : Controller
    {
        private readonly VehicleRepository vehicleRepository;
        private readonly VehicleTypeRepository vehicleTypeRepository;
        private readonly FleetRepository fleetRepository;

        public VehicleController(IConfiguration configuration){
            vehicleRepository = new VehicleRepository(configuration);
            vehicleTypeRepository = new VehicleTypeRepository(configuration);
            fleetRepository = new FleetRepository(configuration);
        }

        // GET: Vehicles
        public ActionResult Index(string chassiFilter = null)
        {
            ViewBag.ListFleets = SelectListFleets();
            var result = vehicleRepository.FindAll(chassiFilter).ToList();
            return View(result);
        }

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Vehicle vehicle = vehicleRepository.FindByID(id.Value);
            if (vehicle == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            ViewBag.ListFleets = SelectListFleets();

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {   
            ViewBag.ListFleets = SelectListFleets();   
            ViewBag.ListVehicleTypes = new SelectList(vehicleTypeRepository.FindAll().ToList(),"Code","Name");

            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Chassi,Type,NumberPassengers,Color")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.IsEnabled = true;
                vehicle.Color = !string.IsNullOrWhiteSpace(vehicle.Color) ? vehicle.Color.ToUpper() : "N/A";
                vehicle.CodeFleet = fleetRepository.FindCurrentFleet();

                vehicleRepository.Add(vehicle);
                return RedirectToAction("Index");
            }
            else{
                ViewBag.ListVehicleTypes = new SelectList(vehicleTypeRepository.FindAll().ToList(),"Code","Name");
                ViewBag.ListFleets = SelectListFleets();                
            }

            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Vehicle vehicle = vehicleRepository.FindByID(id.Value);
            if (vehicle == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            ViewBag.ListFleets = SelectListFleets();   
            ViewBag.ListVehicleTypes = new SelectList(vehicleTypeRepository.FindAll().ToList(),"Code","Name");

            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Chassi,Type,NumberPassengers,Color")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.CodeFleet = fleetRepository.FindCurrentFleet();
                vehicleRepository.Update(vehicle);
                return RedirectToAction("Index");
            }
            else{
                ViewBag.ListFleets = SelectListFleets();   
                ViewBag.ListVehicleTypes = new SelectList(vehicleTypeRepository.FindAll().ToList(),"Code","Name");                
            }

            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Vehicle vehicle = vehicleRepository.FindByID(id.Value);
            if (vehicle == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            vehicleRepository.Remove(id);
            return RedirectToAction("Index");
        }

        private SelectList SelectListFleets(){
            var list = fleetRepository.FindAll().ToList();
            var currentFleet = list.Where(x => x.Current).FirstOrDefault();                     
            var selectList = new SelectList(list,"Code","Name"); 

            if(!string.IsNullOrWhiteSpace(currentFleet?.Code)){
                var selected = selectList.Where(x => x.Value == currentFleet.Code).First();
                selected.Selected = true;
            } 
            
            return selectList;
        }  

        [HttpPost, ActionName("Current")]
        public ActionResult Current(string code)
        {
            if (!string.IsNullOrWhiteSpace(code))           
                fleetRepository.Current(code);
                       
            return Json(true);
        }                
    }
}
