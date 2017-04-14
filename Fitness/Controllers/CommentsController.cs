using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fitness.Models;
using System.Data.Entity;

namespace Fitness.Controllers
{
    public class CommentsController : Controller
    {
        private fitnessEntities db = new fitnessEntities();
        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreatePartial(int ParentCommentID, int PostID)
        {
            var comment = new Comment();
            comment.PostID = PostID;
            comment.ParentCommentID = ParentCommentID;
            return PartialView("CreatePartial", comment);
        }

        public ActionResult Create(Comment comment)
        {
            comment.User = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            //if (comment.ParentCommentID == 0 || comment.ParentCommentID == null)
            //{
            //    return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            //}
            comment.DateRecorded = DateTime.Now;
            db.Comments.Add(comment);
            db.SaveChanges();

            return Redirect("~/Posts/Details/" + comment.PostID + "/#" + comment.CommentID);
        }

        public ActionResult upvote(int id)
        {
            User user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            Comment comment = db.Comments.Where(x => x.CommentID == id).FirstOrDefault();
            var vote = db.CommentVotes.Where(x => x.UserID == user.UserID && x.CommentID == comment.CommentID).FirstOrDefault();
            if (vote != null)
            {
                if (vote.Value == -1)
                {
                    comment.Downvotes--;
                    comment.Upvotes++;
                    vote.Value = 1;
                    db.Entry(vote).State = EntityState.Modified;
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChangesAsync();
                }
                else
                {
                    //undo vote
                    comment.Upvotes--;
                    db.CommentVotes.Remove(vote);
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChangesAsync();
                }
            }
            else
            {
                //create new postvote
                CommentVote newVote = new CommentVote();
                newVote.User = user;
                newVote.Comment = comment;
                newVote.Value = 1;
                comment.Upvotes++;
                db.CommentVotes.Add(newVote);
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChangesAsync();
            }

            return Redirect("/Posts/Details/" + comment.PostID + "/#" + comment.CommentID);
        }

        public PartialViewResult CreateModal()
        {
            return PartialView("CreateModal");
        }

        public ActionResult downvote(int id)
        {
            User user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            Comment comment = db.Comments.Where(x => x.CommentID == id).FirstOrDefault();
            var vote = db.CommentVotes.Where(x => x.UserID == user.UserID && x.CommentID == comment.CommentID).FirstOrDefault();
            if (vote != null)
            {
                if (vote.Value == 1)
                {
                    comment.Downvotes++;
                    comment.Upvotes--;
                    vote.Value = -1;
                    db.Entry(vote).State = EntityState.Modified;
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    //undo vote
                    comment.Downvotes--;
                    db.CommentVotes.Remove(vote);
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                //create new postvote
                CommentVote newVote = new CommentVote();
                newVote.User = user;
                newVote.Comment = comment;
                newVote.Value = -1;
                comment.Downvotes++;
                db.CommentVotes.Add(newVote);
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Redirect("/Posts/Details/" + comment.PostID + "/#" + comment.CommentID);
        }

        public ActionResult Delete(int id)
        {
            if (User.IsInRole("Admin"))
            {
                var comment = db.Comments.Find(id);
                return View(comment);
            }
            else
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = this.Url });
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var comment = db.Comments.Find(id); 
            db.Comments.Remove(comment);
            db.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new { id = comment.PostID });
        }

    }
}