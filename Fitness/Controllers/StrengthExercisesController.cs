﻿using System;
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
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "StrengthExercises/Details/" + id });
            }
            return View(strengthExercise);
        }

        // GET: StrengthExercises/Create
        public ActionResult Create(int? id)
        {
            if (id != null)
            {
                ViewBag.List = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Strength"), "ExerciseListID", "Name", id);
            }
            else
            {
                ViewBag.List = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Strength"), "ExerciseListID", "Name", 0);
            }
            
            return View();
        }

        public ActionResult CreateModal(int id)
        {
            ViewBag.List = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Strength"), "ExerciseListID", "Name", id);
            return PartialView("CreateModal");
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
                return RedirectToAction("Details", "ExerciseLists", new { id = strengthExercise.ExerciseListID });
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
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "StrengthExercises/Edit/" + id });
            }
            ViewBag.List = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID), "ExerciseListID", "Name", strengthExercise.ExerciseListID);
            return View(strengthExercise);
        }

        // POST: StrengthExercises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExerciseID,ExerciseListID,UserID,Rep,Sets,Weight,DateRecorded")] StrengthExercise strengthExercise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(strengthExercise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ExerciseLists", new { id = strengthExercise.ExerciseListID });
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
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "StrengthExercises/Delete/" + id });
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
            return RedirectToAction("Details", "ExerciseLists", new { id = strengthExercise.ExerciseListID });
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
