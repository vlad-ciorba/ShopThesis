using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ForMen.Models;

namespace ForMen.Controllers
{
    public class CartsController : ApiController
    {
        private ForMenContext db = new ForMenContext();

        // GET: api/Carts
        //public IQueryable<Cart> GetCarts()
        //{
        //    return db.Carts;
        //}

        // GET: api/Carts/5
        // Get user cart by user ID
        public Cart GetCart(int id)
        {
            return db.Carts.Where(item => item.UserID == id).FirstOrDefault();
        }

        // PUT: api/Carts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCart(int id, Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cart.ID)
            {
                return BadRequest();
            }

            //db.Entry(cart).State = EntityState.Modified;

            foreach (ProductQuantity cartProduct in cart.Products)
            {
                ProductQuantity existingCartProduct = db.Carts.Find(id).Products.Find(item => item.ProductID == cartProduct.ProductID);
                if (existingCartProduct != null)
                {
                    if (cartProduct.Quantity > 0)
                    {
                        existingCartProduct.Quantity = cartProduct.Quantity;
                    }
                    else
                    {
                        db.ProductQuantities.Remove(existingCartProduct);
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(db.Entry(cart).Entity);
        }

        // POST: api/Carts
        [ResponseType(typeof(Cart))]
        public IHttpActionResult PostCart(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cart userCart = db.Carts.Where(item => item.UserID == cart.UserID).FirstOrDefault();

            foreach (ProductQuantity cartProduct in cart.Products)
            {
                bool exists = false;

                foreach (ProductQuantity userCartProduct in userCart.Products)
                {
                    if (userCartProduct.ProductID == cartProduct.ProductID)
                    {
                        userCartProduct.Quantity += cartProduct.Quantity;
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    userCart.Products.Add(cartProduct);
            }

            db.SaveChanges();
            return Ok(userCart);
            //return CreatedAtRoute("DefaultApi", new { id = cart.ID }, cart);
        }

        // DELETE: api/Carts/5
        [ResponseType(typeof(Cart))]
        public IHttpActionResult DeleteCart(int id)
        {
            Cart cart = db.Carts.Where(item => item.UserID == id).FirstOrDefault();
            if (cart == null)
            {
                return NotFound();
            }

            List<ProductQuantity> cartProducts = cart.Products.Select(item => item).ToList();

            foreach (ProductQuantity cartProduct in cartProducts)
            {
                db.ProductQuantities.Remove(cartProduct);
            }

            //db.Carts.Remove(cart);
            db.SaveChanges();

            return Ok(cart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(int id)
        {
            return db.Carts.Count(e => e.ID == id) > 0;
        }
    }
}