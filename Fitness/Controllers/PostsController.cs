using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fitness.Controllers
{
    public class PostsController : Controller
    {
        private fitnessEntities db = new fitnessEntities();
        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.OrderBy(x => x.DateRecorded).ToList();
            return View(posts);
        }

        public ActionResult Details(int id)
        {
            return View(db.Posts.Find(id));
        }

        public ActionResult upvote(int id)
        {
            Post post = db.Posts.Where(x => x.PostID == id).FirstOrDefault();
            post.Upvotes++;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChangesAsync();
            return Redirect("/Posts/Index/#" + id);
        }

        public ActionResult downvote(int id)
        {
            Post post = db.Posts.Where(x => x.PostID == id).FirstOrDefault();
            post.DownVotes++;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChangesAsync();
            return Redirect("/Posts/Index/#" + id);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Title,Text")]Post model)
        {
            if (ModelState.IsValid)
            {
                model.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                model.UserID = model.User.UserID;
                model.DateRecorded = DateTime.Now;
                db.Posts.Add(model);    
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}