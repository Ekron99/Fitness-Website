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
    public class AerobicGoalsController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: AerobicGoals
        public ActionResult Index()
        {
            var aerobicGoals = db.AerobicGoals.Include(a => a.ExerciseList).Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID);
            return View(aerobicGoals.ToList());
        }

        // GET: AerobicGoals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AerobicGoal aerobicGoal = db.AerobicGoals.Find(id);
            if (aerobicGoal == null)
            {
                return HttpNotFound();
            }
            return View(aerobicGoal);
        }

        // GET: AerobicGoals/Create
        public ActionResult Create()
        {
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.Type == "Aerobic"), "ExerciseListID", "Name");
            List<string> list = new List<string>();
            list.Add("Duration");
            list.Add("Length");
            ViewBag.Focus = new SelectList(list);
            return View();
        }

        // POST: AerobicGoals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExerciseListID,Name,Duration,Length,StartDate,EndDate,Focus")] AerobicGoal aerobicGoal)
        {
            if (aerobicGoal.StartDate > aerobicGoal.EndDate)
            {
                ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.Type == "Aerobic"), "ExerciseListID", "Name");
                return View(aerobicGoal);
            }
            if (ModelState.IsValid)
            {
                aerobicGoal.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                db.AerobicGoals.Add(aerobicGoal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists, "ExerciseListID", "Name", aerobicGoal.ExerciseListID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", aerobicGoal.UserID);
            return View(aerobicGoal);
        }

        public ActionResult ViewProgress(int? id)
        {
            AerobicGoal goal = db.AerobicGoals.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).FirstOrDefault();
            if (goal.Focus == "Duration")
            {
                ViewBag.progress = db.AerobicExercises.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.ExerciseListID == goal.ExerciseListID).Where(x => x.DateRecorded >= goal.StartDate && x.DateRecorded <= goal.EndDate).OrderByDescending(x => x.Duration);
                TimeSpan bestTime = ViewBag.progress[0];
                ViewBag.percentDone = bestTime.TotalSeconds / goal.Duration.TotalSeconds;
                ViewBag.percentDone *= 100;
                ViewBag.percentDone = Math.Round(ViewBag.percentDone, 2);
            }
            else
            {
                //TODO: Make it to where it calculates the completion for the length
                ViewBag.progress = db.AerobicExercises.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.ExerciseListID == goal.ExerciseListID).Where(x => x.DateRecorded >= goal.StartDate && x.DateRecorded <= goal.EndDate).OrderByDescending(x => x.Length);

            }

            

            return View();
        }

        // GET: AerobicGoals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AerobicGoal aerobicGoal = db.AerobicGoals.Find(id);
            if (aerobicGoal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.Type == "Aerobic"), "ExerciseListID", "Name");
            return View(aerobicGoal);
        }

        // POST: AerobicGoals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GoalID,UserID,ExerciseListID,Name,Duration,Length,StartDate,EndDate,Focus")] AerobicGoal aerobicGoal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aerobicGoal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.Type == "Aerobic"), "ExerciseListID", "Name");
            return View(aerobicGoal);
        }

        // GET: AerobicGoals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AerobicGoal aerobicGoal = db.AerobicGoals.Find(id);
            if (aerobicGoal == null)
            {
                return HttpNotFound();
            }
            return View(aerobicGoal);
        }

        // POST: AerobicGoals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AerobicGoal aerobicGoal = db.AerobicGoals.Find(id);
            db.AerobicGoals.Remove(aerobicGoal);
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
