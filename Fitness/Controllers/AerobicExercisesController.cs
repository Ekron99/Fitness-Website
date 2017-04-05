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
    public class AerobicExercisesController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: AerobicExercises
        public ActionResult Index()
        {
            var aerobicExercises = db.AerobicExercises.Include(a => a.User).Include(a => a.ExerciseList).Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID);
            return View(aerobicExercises.ToList());
        }

        // GET: AerobicExercises/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AerobicExercise aerobicExercise = db.AerobicExercises.Find(id);
            if (aerobicExercise == null)
            {
                return HttpNotFound();
            }
            return View(aerobicExercise);
        }

        // GET: AerobicExercises/Create
        public ActionResult Create()
        {
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Aerobic"), "ExerciseListID", "Name");
            AerobicExercise exercise = new AerobicExercise();
            ViewBag .DateRecorded = DateTime.Now;
            return View();
        }

        // POST: AerobicExercises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AerobicExerciseID,UserID,Duration,Length,DateRecorded,ExerciseListID")] AerobicExercise aerobicExercise)
        {
            if (ModelState.IsValid)
            {
                aerobicExercise.UserID = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID;
                db.AerobicExercises.Add(aerobicExercise);
                db.SaveChanges();
                return RedirectToAction("Index", "ExerciseLists");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", aerobicExercise.UserID);
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists, "ExerciseListID", "Name", aerobicExercise.ExerciseListID);
            return View(aerobicExercise);
        }

        // GET: AerobicExercises/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AerobicExercise aerobicExercise = db.AerobicExercises.Find(id);
            if (aerobicExercise == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", aerobicExercise.UserID);
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists, "ExerciseListID", "Name", aerobicExercise.ExerciseListID);
            return View(aerobicExercise);
        }

        // POST: AerobicExercises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AerobicExerciseID,UserID,Duration,Length,DateRecorded,ExerciseListID")] AerobicExercise aerobicExercise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aerobicExercise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", aerobicExercise.UserID);
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists, "ExerciseListID", "Name", aerobicExercise.ExerciseListID);
            return View(aerobicExercise);
        }

        // GET: AerobicExercises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AerobicExercise aerobicExercise = db.AerobicExercises.Find(id);
            if (aerobicExercise == null)
            {
                return HttpNotFound();
            }
            return View(aerobicExercise);
        }

        // POST: AerobicExercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AerobicExercise aerobicExercise = db.AerobicExercises.Find(id);
            db.AerobicExercises.Remove(aerobicExercise);
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
