using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fitness.Models;

namespace Fitness.Controllers
{
    public class FriendsController : Controller
    {
        fitnessEntities db = new fitnessEntities();
        // GET: Friends
        public ActionResult Index()
        {
            var list = db.Users.ToList();
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            list.Remove(user);
            foreach (var item in list)
            {
                var match = db.Friends.Where(x => (x.UserID1 == user.UserID && x.UserID2 == item.UserID) || (x.UserID2 == user.UserID && x.UserID1 == item.UserID)).FirstOrDefault();
                if (match != null)
                {
                    item.friend = match.Status;
                }
                else
                {
                    item.friend = "None";
                }
            }
            return View(list);
        }

        public ActionResult PendingFriends()
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            var list = db.Friends.Where(x => (x.User1.UserID == user.UserID) && x.Status == "Pending").ToList();

            return PartialView("PendingFriends", list);
        }

        public ActionResult ConfirmedFriends()
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            List<User> list = new List<Models.User>();
            foreach (var item in user.Friends)
            {
                if (item.Status != "Confirmed")
                {
                    continue;
                }
                if (item.User.UserID == user.UserID)
                {
                    list.Add(item.User1);
                }
                else
                {
                    list.Add(item.User);
                }
            }

            foreach (var item in user.Friends1)
            {
                if (item.Status != "Confirmed")
                {
                    continue;
                }
                if (item.User.UserID == user.UserID)
                {
                    list.Add(item.User1);
                }
                else
                {
                    list.Add(item.User);
                }
            }
            return PartialView("ConfirmedFriends", list);
        }

        public ActionResult Confirm(int id)
        {
            var pair = db.Friends.Find(id);
            pair.Status = "Confirmed";
            db.Entry(pair).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Deny(int id)
        {
            var pair = db.Friends.Find(id);
            db.Friends.Remove(pair);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Add(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            var friend = db.Users.Find(id);

            var pair1 = db.Friends.Where(x => x.UserID1 == user.UserID && x.UserID2 == friend.UserID).FirstOrDefault();
            var pair2 = db.Friends.Where(x => x.UserID1 == friend.UserID && x.UserID2 == user.UserID).FirstOrDefault();
            if (pair1 == null && pair2 == null)
            {
                Friend match = new Friend();
                match.User = user;
                match.User1 = friend;
                match.DateRecorded = DateTime.Now;
                match.Status = "Pending";
                db.Friends.Add(match);
                db.SaveChanges();
            }
            else
            {
                ViewBag.error = "Uh oh..seems like you are already friends!";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            var friend = db.Users.Find(id);

            var pair1 = db.Friends.Where(x => x.UserID1 == user.UserID && x.UserID2 == friend.UserID).FirstOrDefault();
            var pair2 = db.Friends.Where(x => x.UserID1 == friend.UserID && x.UserID2 == user.UserID).FirstOrDefault();

            if (pair1 != null)
            {
                db.Friends.Remove(pair1);
                db.SaveChanges();
            }
            else if (pair2 != null)
            {
                db.Friends.Remove(pair2);
                db.SaveChanges();
            }
            else
            {
                ViewBag.error = "Uh oh... looks like you aren't friends with this person";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Friends()
        {

            return PartialView();
        }
    }
}