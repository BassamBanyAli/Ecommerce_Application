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
    public class ShoppingCartItemsController : Controller
    {
        private EcommerceEntities1 db = new EcommerceEntities1();

        // GET: ShoppingCartItems
        public ActionResult Index()
        {
            var shoppingCartItems = db.ShoppingCartItems.Include(s => s.Product).Include(s => s.ShoppingCart);
            return View(shoppingCartItems.ToList());
        }

        // GET: ShoppingCartItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCartItem shoppingCartItem = db.ShoppingCartItems.Find(id);
            if (shoppingCartItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCartItem);
        }

        // GET: ShoppingCartItems/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            ViewBag.CartID = new SelectList(db.ShoppingCarts, "CartID", "CartID");
            return View();
        }

        // POST: ShoppingCartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartItemID,CartID,ProductID,Quantity,CreatedAt")] ShoppingCartItem shoppingCartItem)
        {
            if (ModelState.IsValid)
            {
                shoppingCartItem.CreatedAt = DateTime.Now;
                db.ShoppingCartItems.Add(shoppingCartItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", shoppingCartItem.ProductID);
            ViewBag.CartID = new SelectList(db.ShoppingCarts, "CartID", "CartID", shoppingCartItem.CartID);
            return View(shoppingCartItem);
        }

        // GET: ShoppingCartItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCartItem shoppingCartItem = db.ShoppingCartItems.Find(id);
            if (shoppingCartItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", shoppingCartItem.ProductID);
            ViewBag.CartID = new SelectList(db.ShoppingCarts, "CartID", "CartID", shoppingCartItem.CartID);
            return View(shoppingCartItem);
        }

        // POST: ShoppingCartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartItemID,CartID,ProductID,Quantity,CreatedAt")] ShoppingCartItem shoppingCartItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingCartItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", shoppingCartItem.ProductID);
            ViewBag.CartID = new SelectList(db.ShoppingCarts, "CartID", "CartID", shoppingCartItem.CartID);
            return View(shoppingCartItem);
        }

        // GET: ShoppingCartItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCartItem shoppingCartItem = db.ShoppingCartItems.Find(id);
            if (shoppingCartItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCartItem);
        }

        // POST: ShoppingCartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingCartItem shoppingCartItem = db.ShoppingCartItems.Find(id);
            db.ShoppingCartItems.Remove(shoppingCartItem);
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
