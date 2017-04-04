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
    public class StrengthGoalController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: StrengthGoal
        public ActionResult Index()
        {
            var strengthGoals = db.StrengthGoals.Include(s => s.ExerciseList).Where(x => x.ExerciseList.Type == "Strength");
            return View(strengthGoals.ToList());
        }

        public ActionResult ViewProgress(int? id)
        {
            //TODO: be able to add a new entry on this page
            StrengthGoalProgressModel model = new StrengthGoalProgressModel();
            StrengthGoal goal = db.StrengthGoals.Where(x => x.GoalID == id).FirstOrDefault();
            if (goal == null)
            {
                return RedirectToAction("Unauthorized", "Users");
            }
            model.goal = goal;
            model.progress = db.StrengthExercises.Where(x => x.User.Email == User.Identity.Name).Where(x => x.ExerciseListID == goal.ExerciseListID).Where(x => x.DateRecorded <= goal.EndDate).OrderByDescending(x => x.Weight).OrderByDescending(x => x.DateRecorded);
            decimal progress = 0;
            foreach (var item in model.progress)
            {
                if (item.Weight > progress)
                {
                    progress = item.Weight;
                }
            }
            ViewBag.progress = progress;
            ViewBag.percentDone = progress / goal.TargetWeight * 100;
            ViewBag.percentDone = Math.Round(ViewBag.percentDone, 2);
            return View(model);
        }

        // GET: StrengthGoal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StrengthGoal strengthGoal = db.StrengthGoals.Find(id);
            if (strengthGoal == null)
            {
                return RedirectToAction("Unauthorized", "Users");
            }
            return View(strengthGoal);
        }

        // GET: StrengthGoal/Create
        public ActionResult Create()
        {
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Strength"), "ExerciseListID", "Name");
            return View();
        }

        // POST: StrengthGoal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GoalID,ExerciseListID,Name,TargetWeight,EndDate")] StrengthGoal strengthGoal)
        {
            if (ModelState.IsValid)
            {
                strengthGoal.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                db.StrengthGoals.Add(strengthGoal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Strength"), "ExerciseListID", "Name", strengthGoal.ExerciseListID);
            return View(strengthGoal);
        }

        // GET: StrengthGoal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StrengthGoal strengthGoal = db.StrengthGoals.Find(id);
            if (strengthGoal == null)
            {
                return RedirectToAction("Unauthorzied", "Users");
            }
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Strength"), "ExerciseListID", "Name", strengthGoal.ExerciseListID);
            return View(strengthGoal);
        }

        // POST: StrengthGoal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GoalID,ExerciseListID,Name,TargetWeight,EndDate")] StrengthGoal strengthGoal)
        {
            if (ModelState.IsValid)
            {
                strengthGoal.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                strengthGoal.UserID = strengthGoal.User.UserID;
                db.Entry(strengthGoal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Strength"), "ExerciseListID", "Name", strengthGoal.ExerciseListID);
            return View(strengthGoal);
        }

        // GET: StrengthGoal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StrengthGoal strengthGoal = db.StrengthGoals.Find(id);
            if (strengthGoal == null)
            {
                return RedirectToAction("Unauthoized", "Users");
            }
            return View(strengthGoal);
        }

        // POST: StrengthGoal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StrengthGoal strengthGoal = db.StrengthGoals.Find(id);
            db.StrengthGoals.Remove(strengthGoal);
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
