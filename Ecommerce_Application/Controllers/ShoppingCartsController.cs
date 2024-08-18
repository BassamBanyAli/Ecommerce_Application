using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce_Application.Models;

namespace Ecommerce_Application.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private EcommerceEntities1 db = new EcommerceEntities1();

        // GET: ShoppingCarts
        public ActionResult Index()
        {
            var LoggedUser = Session["User"] as User;
            if (LoggedUser == null)
            {
                // Handle the case where the user is not logged in or session is not set
                return RedirectToAction("Login", "Users"); // Redirect to login page or handle accordingly
            }
            var userId = LoggedUser.ID;

            var cart = db.ShoppingCarts.SingleOrDefault(c => c.UserID == userId);
            if (cart == null)
            {
                // Handle the case where no cart is found
                return HttpNotFound("Shopping cart not found for the user.");
            }

            if (cart != null)
            {
                var items = db.ShoppingCartItems.Where(i => i.CartID == cart.CartID).ToList();
                ViewBag.Cart = cart;
                return View(items);
            }

            return View(new List<ShoppingCartItem>());
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCart(List<ShoppingCartItem> CartItems)
        {
            // Retrieve the current user's shopping cart
            var loggedUser = Session["User"] as User;
            if (loggedUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = db.ShoppingCarts.SingleOrDefault(c => c.UserID == loggedUser.ID);
            if (cart == null)
            {
                return HttpNotFound();
            }

            foreach (var item in CartItems)
            {
                // Find the existing cart item
                var existingItem = db.ShoppingCartItems.SingleOrDefault(i => i.CartItemID == item.CartItemID && i.CartID == cart.CartID);
                if (existingItem != null)
                {
                    // Update the quantity
                    existingItem.Quantity = item.Quantity;
                }
            }

            db.SaveChanges();

                return RedirectToAction("Index");
   
        }

        // GET: ShoppingCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name");
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int userId)
        {
            // Check if a shopping cart already exists for this user
            var existingCart = db.ShoppingCarts.FirstOrDefault(c => c.UserID == userId);

            if (existingCart == null)
            {
                // Create a new shopping cart for the user
                ShoppingCart shoppingCart = new ShoppingCart
                {
                    UserID = userId,
                    CreatedAt = DateTime.Now // Set the default value for CreatedAt
                };

                db.ShoppingCarts.Add(shoppingCart);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                // If a cart already exists, redirect to the index or show an appropriate message
                return RedirectToAction("Index");
            }
        }

        // GET: ShoppingCarts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", shoppingCart.UserID);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartID,UserID,CreatedAt")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", shoppingCart.UserID);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            db.ShoppingCarts.Remove(shoppingCart);
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
