using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fitness.Controllers
{
    public class GoalsController : Controller
    {
        fitnessEntities db = new fitnessEntities();
        // GET: Goals
        public ActionResult Index()
        {
            GoalsModel goals = new GoalsModel();
            goals.StrengthGoals = db.StrengthGoals.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).ToList();db.StrengthGoals.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).ToList();
            goals.AerobicGoals = db.AerobicGoals.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).ToList();

            return View(goals);
        }

        public PartialViewResult CreateAerobicModal()
        {
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.Type == "Aerobic"), "ExerciseListID", "Name");
            List<string> list = new List<string>();
            list.Add("Duration");
            list.Add("Distance");
            ViewBag.Focus = new SelectList(list);
            return PartialView("CreateAerobicModal");
        }
        
        public PartialViewResult CreateStrengthModal()
        {
            ViewBag.ExerciseListID = new SelectList(db.ExerciseLists.Where(x => x.UserID == db.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault().UserID).Where(n => n.Type == "Strength"), "ExerciseListID", "Name");
            return PartialView("CreateStrengthModal");
        }

    }
}