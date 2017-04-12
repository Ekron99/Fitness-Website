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
            User user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            foreach (var item in posts)
            {
                var vote = item.PostVotes.Where(x => x.UserID == user.UserID).FirstOrDefault();
                if (vote != null)
                {
                    item.userVote = vote.Value;
                }
                else
                {
                    item.userVote = 0;
                }
            }
            return View(posts);
        }

        public ActionResult Details(int id)
        {
            var post = db.Posts.Find(id);
            User user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            foreach(var item in post.Comments)
            {
                //look for the users vote for the specific comment
                var vote = item.CommentVotes.Where(x => x.UserID == user.UserID).FirstOrDefault();
                if (vote != null)
                {
                    item.userVote = vote.Value;
                }
                else
                {
                    item.userVote = 0;
                }
            }

            post.Comments.OrderBy(x => x.ParentCommentID == null).OrderBy(x => x.DateRecorded);
            return View(post);
        }

        public ActionResult upvote(int id)
        {
            User user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            Post post = db.Posts.Where(x => x.PostID == id).FirstOrDefault();
            var vote = db.PostVotes.Where(x => x.UserID == user.UserID && x.PostID == post.PostID).FirstOrDefault();
            if (vote != null)
            {
                if (vote.Value == -1)
                {
                    post.DownVotes--;
                    post.Upvotes++;
                    vote.Value = 1;
                    db.Entry(vote).State = EntityState.Modified;
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChangesAsync();
                }                
            }
            else
            {
                //create new postvote
                PostVote newVote = new PostVote();
                newVote.User = user;
                newVote.Post = post;
                newVote.Value = 1;
                post.Upvotes++;
                db.PostVotes.Add(newVote);
                db.Entry(post).State = EntityState.Modified;
                db.SaveChangesAsync();
            }
           
            return Redirect("/Posts/Index/#" + id);
        }

        public PartialViewResult CreateModal()
        {
            return PartialView("CreateModal");
        }

        public ActionResult downvote(int id)
        {
            User user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            Post post = db.Posts.Where(x => x.PostID == id).FirstOrDefault();
            var vote = db.PostVotes.Where(x => x.UserID == user.UserID && x.PostID == post.PostID).FirstOrDefault();
            if (vote != null)
            {
                if (vote.Value == 1)
                {
                    post.DownVotes++;
                    post.Upvotes--;
                    vote.Value = -1;
                    db.Entry(vote).State = EntityState.Modified;
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    //undo vote
                    
                }
            }
            else
            {
                //create new postvote
                PostVote newVote = new PostVote();
                newVote.User = user;
                newVote.Post = post;
                newVote.Value = -1;
                post.DownVotes++;
                db.PostVotes.Add(newVote);
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }

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