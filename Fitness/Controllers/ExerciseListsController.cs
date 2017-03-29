using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fitness.Models;
using System.Web.Helpers;

namespace Fitness.Controllers
{
    public class ExerciseListsController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: ExerciseLists
        public ActionResult Index()
        {
            StrengthAerobicList list = new StrengthAerobicList();
            list.strengthExercise = db.ExerciseLists.Where(e => e.UserID == db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.Type == "Strength").ToList();
            list.aerobicExercise = db.ExerciseLists.Where(e => e.UserID == db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.Type == "Aerobic").ToList();
            return View(list);
        }

        // GET: ExerciseLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseList exerciseList = db.ExerciseLists.Find(id);
            ViewBag.NoStrength = false;
            ViewBag.NoAerobic = false;
            if (exerciseList.StrengthExercises.Count == 0)
            {
                ViewBag.NoStrength = true;
            }
            if (exerciseList.AerobicExercises.Count == 0)
            {
                ViewBag.NoAerobic = true;
            }
            if (exerciseList == null)
            {
                return HttpNotFound();
            }
            return View(exerciseList);
        }

        public ActionResult CreateChart(ExerciseList exercise)
        {
            List<string> xData = new List<string>();
            List<decimal> yData = new List<decimal>();
            exercise = db.ExerciseLists.Find(exercise.ExerciseListID);
            if (exercise.StrengthExercises.Count == 0)
            {
                return null;
            }
            decimal maxWeight = 0;
            DateTime currentDay = exercise.StrengthExercises.ElementAt(0).DateRecorded;
            foreach (var item in exercise.StrengthExercises)
            {
                if (currentDay < item.DateRecorded)
                {
                    xData.Add(currentDay.ToShortDateString().ToString());
                    yData.Add(maxWeight);
                    currentDay = item.DateRecorded;
                    maxWeight = 0;
                }
                if (maxWeight < item.Weight)
                {
                    maxWeight = item.Weight;
                }
                
                
            }
            xData.Add(currentDay.ToShortDateString().ToString());
            yData.Add(maxWeight);
            ViewBag.NoImage = false;
            Chart chart = new Chart(800, 600);
            chart.AddLegend("Weight");
            chart.AddTitle(exercise.Name + " History");
            chart.AddSeries(xValue: xData, yValues: yData, chartType: "Line");
            chart.Write("png");
            return null;
        }

        // GET: ExerciseLists/Create
        public ActionResult Create()
        {
            List<string> types = new List<string>();
            types.Add("Strength");
            types.Add("Aerobic");
            ViewBag.typelist = new SelectList(types);
            return View();
        }

        // POST: ExerciseLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExerciseListID,Name,UserID,Type")] ExerciseList exerciseList)
        {
            if (ModelState.IsValid)
            {
                
                exerciseList.UserID = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID;
                db.ExerciseLists.Add(exerciseList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exerciseList);
        }

        // GET: ExerciseLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseList exerciseList = db.ExerciseLists.Find(id);
            if (exerciseList == null)
            {
                return RedirectToAction("Unauthorized", "Users");
            }
            List<string> types = new List<string>();
            types.Add("Strength");
            types.Add("Aerobic");
            ViewBag.typelist = new SelectList(types);
            if (exerciseList == null)
            {
                return HttpNotFound();
            }
            return View(exerciseList);
        }

        // POST: ExerciseLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExerciseListID,Name,UserID, Type")] ExerciseList exerciseList)
        {
            if (ModelState.IsValid)
            {
                exerciseList.UserID = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID;
                db.Entry(exerciseList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exerciseList);
        }

        // GET: ExerciseLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseList exerciseList = db.ExerciseLists.Find(id);
            if (exerciseList == null)
            {
                return HttpNotFound();
            }
            return View(exerciseList);
        }

        // POST: ExerciseLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExerciseList exerciseList = db.ExerciseLists.Find(id);
            db.ExerciseLists.Remove(exerciseList);
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
