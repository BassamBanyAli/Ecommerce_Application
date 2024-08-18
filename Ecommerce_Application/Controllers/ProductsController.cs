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
    public class ProductsController : Controller
    {
        private EcommerceEntities1 db = new EcommerceEntities1();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Price,Image_URL,Quantity,Description,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {

                if (product.Image_URL == null)
                {
                    product.Image_URL = "Default_Product.png"; // Optionally set to a specific default image path
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View(product);
        }


        public ActionResult AddToCart(int?id)
        {
            if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }

    var product = db.Products.Find(id);
    if (product == null)
    {
        return HttpNotFound();
    }
            var loggedUser = Session["User"] as User;
            if (loggedUser == null)
            {
                // Redirect to login if user is not logged in
                return RedirectToAction("Login", "Users");
            }

            var cart = db.ShoppingCarts.SingleOrDefault(c => c.UserID == loggedUser.ID);
            if (cart == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var existingItem = db.ShoppingCartItems.SingleOrDefault(i => i.CartID == cart.CartID && i.ProductID == product.ID);
            if (existingItem != null)
            {
                // Update the quantity if the product is already in the cart
                existingItem.Quantity += 1; // Or any quantity you want to add
            }
            else
            {
                // Add new item to the cart
                var newItem = new ShoppingCartItem
                {
                    CartID = cart.CartID,
                    ProductID = product.ID,
                    Quantity = 1, // Or any quantity you want to add
                    CreatedAt = DateTime.Now
                };
                db.ShoppingCartItems.Add(newItem);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    

            // GET: Products/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Price,Image_URL,Quantity,Description,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
