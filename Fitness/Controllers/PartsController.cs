using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fitness.Models;
using System.Web.UI.DataVisualization.Charting;
using System.IO;

namespace Fitness.Controllers
{
    public class PartsController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: Parts
        public ActionResult Index()
        {
            var parts = db.Parts.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID);
            return View(parts.ToList());
        }

        // GET: Parts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "Parts/Details/" + id });
            }

            if (part.UserID != db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID)
            {
                return Redirect("~/Shared/Unauthorized");
            }
            ViewBag.NoMeasurements = false;
            if (part.Measurements.Count == 0)
            {
                ViewBag.NoMeasurements = true;
            }

            return View(part);
        }

        [HttpPost]
        public JsonResult CreateChart(int id)
        {
            List<string> xData = new List<string>();
            List<decimal> yData = new List<decimal>();
            var part = db.Parts.Find(id);
            DateTime currentDay = part.Measurements.OrderBy(x => x.DateRecorded).FirstOrDefault().DateRecorded;
            decimal dailyMax = 0;
            foreach (var item in part.Measurements.OrderBy(x => x.DateRecorded))
            {
                if (item.DateRecorded == currentDay)
                {
                    if (item.Value > dailyMax)
                    {
                        dailyMax = item.Value;
                    }
                }
                else
                {
                    xData.Add(currentDay.ToShortDateString());
                    yData.Add(dailyMax);
                    dailyMax = item.Value;
                    currentDay = item.DateRecorded;
                }
                
            }

            var data = new ChartData();
            data.labels = xData;
            data.data = yData;
            return Json(data);

        }

        // GET: Parts/Create
        public ActionResult Create()
        {

            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Part part)
        {
            if (ModelState.IsValid)
            {
                part.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                db.Parts.Add(part);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", part.UserID);
            return View(part);
        }

        // GET: Parts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "Parts/Edit/" + id });
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", part.UserID);
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PartID,UserID,Name")] Part part)
        {
            if (ModelState.IsValid)
            {
                db.Entry(part).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", part.UserID);
            return View(part);
        }

        // GET: Parts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "Parts/Delete/" + id });
            }
            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Part part = db.Parts.Find(id);
            db.Parts.Remove(part);
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
