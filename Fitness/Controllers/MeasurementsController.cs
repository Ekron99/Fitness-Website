using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fitness.Models;

namespace Fitness.Controllers
{
    public class MeasurementsController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: Measurements
        public ActionResult Index()
        {
            var measurements = db.Measurements;
            return View(measurements.ToList());
        }

        // GET: Measurements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Measurement measurement = db.Measurements.Find(id);
            if (measurement == null)
            {
                return HttpNotFound();
            }
            return View(measurement);
        }

        // GET: Measurements/Create
        public ActionResult Create()
        {
            List<object> selectList = new List<object>();
            //go through each measurement related to the current user
            foreach (var item in db.Parts.Where(x => x.UserID == db.Users.Where(n => n.Email == User.Identity.Name).FirstOrDefault().UserID))
            {
                selectList.Add(new
                {
                    Id = item.PartID,
                    Part = item,
                    PartName = item.Name
                });
            }
            ViewBag.Parts = new SelectList(selectList, "Id", "PartName");
            return View();
        }

        
        public ActionResult CreatePart()
        {
            //create part
            PartList parts = new PartList();
            parts.createdParts = new List<Part>();
            parts.createdParts = db.Parts.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).ToList();
            return View(parts);
        }
        [HttpPost]
        public ActionResult CreatePart(PartList measure)
        {
            User user = db.Users.Where(x => x.Email == User.Identity.Name).First();
            measure.part.UserID = user.UserID;

            db.Parts.Add(measure.part);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        // POST: Measurements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Measurement measurement)
        {
            if (ModelState.IsValid)
            {
                measurement.UserID = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID;
                measurement.DateRecorded = DateTime.Now;
                db.Measurements.Add(measurement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", measurement.UserID);
            return View(measurement);
        }

        // GET: Measurements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Measurement measurement = db.Measurements.Find(id);
            if (measurement == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", measurement.UserID);
            return View(measurement);
        }

        // POST: Measurements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MeasurementID,UserID,Part,Value")] Measurement measurement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(measurement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", measurement.UserID);
            return View(measurement);
        }

        // GET: Measurements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Measurement measurement = db.Measurements.Find(id);
            if (measurement == null)
            {
                return HttpNotFound();
            }
            return View(measurement);
        }

        // POST: Measurements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Measurement measurement = db.Measurements.Find(id);
            db.Measurements.Remove(measurement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
