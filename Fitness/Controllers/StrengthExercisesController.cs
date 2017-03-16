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
    public class StrengthExercisesController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: StrengthExercises
        public ActionResult Index()
        {
            var strengthExercises = db.StrengthExercises.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID);
            return View(strengthExercises.ToList());
        }

        // GET: StrengthExercises/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StrengthExercise strengthExercise = db.StrengthExercises.Find(id);
            if (strengthExercise == null)
            {
                return HttpNotFound();
            }
            return View(strengthExercise);
        }

        // GET: StrengthExercises/Create
        public ActionResult Create()
        {
            ViewBag.List = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Strength"), "ExerciseListID", "Name", 0);
            StrengthExercise exercise = new StrengthExercise();
            exercise.DateRecorded = DateTime.Now;
            return View(exercise);
        }

        // POST: StrengthExercises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExerciseID, ExerciseListID,Rep,Sets,Weight,DateRecorded")] StrengthExercise strengthExercise)
        {
            if (ModelState.IsValid)
            {
                strengthExercise.UserID = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID;
                db.StrengthExercises.Add(strengthExercise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.List = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID), "ExerciseListID", "Name");
            return View(strengthExercise);
        }

        // GET: StrengthExercises/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StrengthExercise strengthExercise = db.StrengthExercises.Find(id);
            if (strengthExercise == null)
            {
                return HttpNotFound();
            }
            ViewBag.List = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID), "ExerciseListID", "Name");
            return View(strengthExercise);
        }

        // POST: StrengthExercises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExerciseID,UserID,Rep,Sets,Weight,DateRecorded")] StrengthExercise strengthExercise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(strengthExercise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(strengthExercise);
        }

        // GET: StrengthExercises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StrengthExercise strengthExercise = db.StrengthExercises.Find(id);
            if (strengthExercise == null)
            {
                return HttpNotFound();
            }
            return View(strengthExercise);
        }

        // POST: StrengthExercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StrengthExercise strengthExercise = db.StrengthExercises.Find(id);
            db.StrengthExercises.Remove(strengthExercise);
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
