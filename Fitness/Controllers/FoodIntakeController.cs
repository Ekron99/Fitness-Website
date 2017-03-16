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
    public class FoodIntakeController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: FoodIntakes
        public ActionResult Index()
        {
            var foodIntakes = db.FoodIntakes.Include(f => f.Food).Where(x => x.User == db.Users.Where(i => i.Email == User.Identity.Name));
            return View(foodIntakes.ToList());
        }

        // GET: FoodIntakes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodIntake foodIntake = db.FoodIntakes.Find(id);
            if (foodIntake == null)
            {
                return HttpNotFound();
            }
            return View(foodIntake);
        }

        // GET: FoodIntakes/Create
        public ActionResult Create()
        {
            //TODO: Add date picker for DateRecorded field
            ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "Name");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: FoodIntakes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodID,DateRecorded,Servings")] FoodIntake foodIntake)
        {
            if (ModelState.IsValid)
            {
                foodIntake.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                db.FoodIntakes.Add(foodIntake);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "Name", foodIntake.FoodID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", foodIntake.UserID);
            return View(foodIntake);
        }

        // GET: FoodIntakes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodIntake foodIntake = db.FoodIntakes.Find(id);
            if (foodIntake.UserID != db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID)
            {
                return RedirectToAction("Unauthorized", "Users", new { ReturnURL = "~/foodIntake/Edit/" + id });
            }
            if (foodIntake == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "Name", foodIntake.FoodID);
            return View(foodIntake);
        }

        // POST: FoodIntakes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FoodID,DateRecorded,Servings")] FoodIntake foodIntake)
        {
            if (ModelState.IsValid)
            {
                foodIntake.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                db.Entry(foodIntake).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "Name", foodIntake.FoodID);
            return View(foodIntake);
        }

        // GET: FoodIntakes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodIntake foodIntake = db.FoodIntakes.Find(id);
            if (foodIntake == null)
            {
                return HttpNotFound();
            }
            return View(foodIntake);
        }

        // POST: FoodIntakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FoodIntake foodIntake = db.FoodIntakes.Find(id);
            db.FoodIntakes.Remove(foodIntake);
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
