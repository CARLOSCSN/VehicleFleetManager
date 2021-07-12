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
    public class FleetController : Controller
    {
        private readonly FleetRepository fleetRepository;

        public FleetController(IConfiguration configuration){
            fleetRepository = new FleetRepository(configuration);
        }

        // GET: Fleets
        public ActionResult Index()
        {
            ViewBag.ListFleets = SelectListFleets();
            return View(fleetRepository.FindAll().ToList());
        }

        // GET: Fleets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Fleet Fleet = fleetRepository.FindByID(id.Value);
            if (Fleet == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            ViewBag.ListFleets = SelectListFleets(); 

            return View(Fleet);
        }

        // GET: Fleets/Create
        public ActionResult Create()
        {      
            return View();
        }

        // POST: Fleets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Code,Name")] Fleet Fleet)
        {
            if (ModelState.IsValid)
            {
                Fleet.IsEnabled = true;
                Fleet.Current = false;

                fleetRepository.Add(Fleet);
                return RedirectToAction("Index");
            }
            else{
                ViewBag.ListFleets = SelectListFleets();                
            }

            return View(Fleet);
        }

        // GET: Fleets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);

            Fleet Fleet = fleetRepository.FindByID(id.Value);
            if (Fleet == null)
                return StatusCode(StatusCodes.Status404NotFound);

            var currentFleet = fleetRepository.FindCurrentFleet();
            if(Fleet?.Code == currentFleet)
                return StatusCode(StatusCodes.Status400BadRequest);

            return View(Fleet);
        }

        // POST: Fleets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Name,Code")] Fleet fleet)
        {
            if (ModelState.IsValid)
            {
                fleetRepository.Update(fleet);
                return RedirectToAction("Index");
            }
            else{
                ViewBag.ListFleets = SelectListFleets();
            }
            
            return View(fleet);
        }

        // GET: Fleets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);

            Fleet Fleet = fleetRepository.FindByID(id.Value);
            if (Fleet == null)
                return StatusCode(StatusCodes.Status404NotFound);

            var currentFleet = fleetRepository.FindCurrentFleet();
            if(Fleet?.Code == currentFleet)
                return StatusCode(StatusCodes.Status400BadRequest);
            
            return View(Fleet);
        }

        // POST: Fleets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            fleetRepository.Remove(id);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Current")]
        public ActionResult Current(string code)
        {
            if (!string.IsNullOrWhiteSpace(code))           
                fleetRepository.Current(code);
                       
            return Json(true);
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
    }
}
