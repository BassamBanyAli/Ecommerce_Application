using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Ecommerce_Application.Models;

namespace Ecommerce_Application.Controllers
{
    public class UsersController : Controller
    {
        private EcommerceEntities1 db = new EcommerceEntities1();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Image == null)
                {
                    user.Image = "Default.png"; // Optionally set to a specific default image path
                }
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }



        // GET: Users/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ID,Name,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Image == null)
                {
                    user.Image = "Default.png"; // Optionally set to a specific default image path
                }
                ShoppingCart shoppingCart = new ShoppingCart
                {
                    UserID = user.ID,
                    CreatedAt = DateTime.Now // Set the default value for CreatedAt
                };

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }


        // GET: Users/Create
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                // Query the user by email
                var loggedInUser = db.Users.SingleOrDefault(u => u.Email == user.Email);

                if (loggedInUser != null && VerifyPassword(user.Password, loggedInUser.Password))
                {
                    // Store the user in session
                    Session["User"] = loggedInUser;
                    ViewBag.IsLogged = true;



                    // Redirect to the Index action in the HomeController
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Add an error message if the user is not found or password does not match
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }

            // If we got this far, something went wrong, redisplay form
            return View(user);
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return inputPassword == storedPassword;
        }


         public ActionResult LogOut()
        {

            Session["User"]=null;
            return RedirectToAction("Index","Home");
        }

        public ActionResult ProfilePage(int? id)
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




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilePage([Bind(Include = "ID,Name,Email,Password,Image")] User user)
        {

            if (ModelState.IsValid)
            {
                if (user.Image == null)
                {
                    user.Image = "Default.png"; // Optionally set to a specific default image path

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Email,Password,Image")] User user)
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
