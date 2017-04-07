using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fitness.Models;
using System.Security.Cryptography;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fitness.Controllers
{
    public class UsersController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.password == model.confirmPassword) { 
                    //make salt and hash first!
                    RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                    byte[] salt = new byte[20];
                    crypto.GetNonZeroBytes(salt);
                    string stringSalt = Convert.ToBase64String(salt);
                    string hash = getHash(model.password, stringSalt);

                    //add user to database
                    User user = new User();
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.Hash = hash;
                    user.Salt = stringSalt;
                    user.Roles = "User";
                    db.Users.Add(user);
                    db.SaveChanges();
                    user = db.Users.Where(x => x.Email == model.Email).First();
                    //login the user in   
                    FormsAuthentication.SetAuthCookie(model.Email, false);

                    return RedirectToAction("Index", "Home", null);
                }
            }

            return View(model);
        }

        public string getHash(string password, string salt)
        {
            password = password + salt;
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            SHA256Managed sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(passwordBytes);
            System.Text.StringBuilder returnHash = new System.Text.StringBuilder();
            for (int n = 0; n < hash.Length; n++)
            {
                returnHash.Append(hash[n]);
            }
            return returnHash.ToString();
        }

        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Index", "Home", null);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel user, string ReturnUrl)
        {
            User existingUser = db.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            if (existingUser == null)
            {
                ViewBag.errorMessage = "User does not exist!";
                return View(user);
            }
            string result = getHash(user.Password, existingUser.Salt);
            if (result == existingUser.Hash)
            {
                FormsAuthentication.SetAuthCookie(user.Email, user.RememberMe);
                if (ReturnUrl != null)
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home", null);
                }
                
            } else
            {
                ViewBag.errorMessage = "Invalid username or password";
                return View(user);
            }

        }

        [AllowAnonymous]
        public ActionResult Unauthorized(string returnURL)
        {
            ViewBag.returnURL = returnURL;
            return View();
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Email,Salt,Hash")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
