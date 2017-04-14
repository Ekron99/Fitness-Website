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
            var posts = db.Posts.OrderByDescending(x => x.DateRecorded).OrderByDescending(x => x.Upvotes - x.DownVotes).ToList();
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
            var userVote = db.PostVotes.Where(x => x.UserID == user.UserID && x.PostID == id).FirstOrDefault();
            if (userVote != null)
            {
                post.userVote = userVote.Value;
            }
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
            post.Comments.OrderBy(x => x.ParentCommentID == null).OrderByDescending(x => x.DateRecorded).OrderByDescending(x => x.Upvotes - x.Downvotes);
            return View(post);
        }

        public ActionResult upvote(int id, bool fromComments = false)
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
                else
                {
                    post.Upvotes--;
                    db.PostVotes.Remove(vote);
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
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

            if (fromComments)
            {
                return Redirect("/Posts/Details/" + id);
            }
            else
            {
                return Redirect("/Posts/Index/#" + id);
            }
        }

        public PartialViewResult CreateModal()
        {
            return PartialView("CreateModal");
        }

        public ActionResult downvote(int id, bool fromComments = false)
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
                    post.DownVotes--;
                    db.PostVotes.Remove(vote);
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
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
            if (fromComments)
            {
                return Redirect("/Posts/Details/" + id);
            }
            else
            {
                return Redirect("/Posts/Index/#" + id);
            }
            
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

        public ActionResult Close(int id)
        {
            if (User.IsInRole("Admin"))
            {
                var post = db.Posts.Find(id);
                return View(post);
            }
            else
            {
                return RedirectToAction("Unauthorized", "Users");
            }
        }

        [HttpPost, ActionName("Close")]
        public ActionResult CloseConfirmed(int id)
        {
            var post = db.Posts.Find(id);
            post.Closed = true;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChangesAsync();
            return RedirectToAction("Index", "Posts");
        }

        public ActionResult Delete(int id)
        {
            if (User.IsInRole("Admin"))
            {
                var post = db.Posts.Find(id);
                return View(post);
            }
            else
            {
                return RedirectToAction("Unauthorized", "Users");
            }
            
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var post = db.Posts.Find(id);
            post.Deleted = true;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChangesAsync();
            return RedirectToAction("Index", "Posts");
        }
    }
}