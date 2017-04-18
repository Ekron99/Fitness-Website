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
    public class FoodsController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: Foods
        public ActionResult Index()
        {
            return View(db.Foods.ToList());
        }

        // GET: Foods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // GET: Foods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodID,publicFood,Name,Quantity,Calories,CaloriesFromFat,SaturatedFat,Cholestoral,Sodium,Potassium,TotalCarbohydrate,DietaryFiber,Sugars,Protein")] Food food)
        {
            if (ModelState.IsValid)
            {
                if (food.publicFood && User.IsInRole("Admin"))
                {
                    food.User = null;
                }
                else
                {
                    food.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                }
                
                db.Foods.Add(food);
                db.SaveChanges();
                return RedirectToAction("Index", "FoodIntake");
            }

            return View(food);
        }

        // GET: Foods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food.UserID == null && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "Foods/Edit/" + id });
            }
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoodID,Name,Quantity,Calories,CaloriesFromFat,SaturatedFat,Cholestoral,Sodium,Potassium,TotalCarbohydrate,DietaryFiber,Sugars,Protein")] Food food)
        {
            if (ModelState.IsValid)
            {
                if (food.publicFood && User.IsInRole("Admin"))
                {
                    food.User = null;
                }
                else
                {
                    food.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                }
                db.Entry(food).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(food);
        }

        // GET: Foods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = db.Foods.Find(id);
            if (food.publicFood && User.IsInRole("Admin"))
            {
                food.User = null;
            }
            else
            {
                food.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            }
            db.Foods.Remove(food);
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
