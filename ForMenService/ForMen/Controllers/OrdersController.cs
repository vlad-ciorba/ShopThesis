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
    public class OrdersController : ApiController
    {
        private ForMenContext db = new ForMenContext();

        // GET: api/Orders
        public IEnumerable<Order> GetOrders()
        {
                return db.Orders.OrderByDescending(item => item.Date);
        }

        // GET: api/Orders/5
        // Get user orders by user ID
        public IEnumerable<Order> GetOrder(int id)
        {
            return db.Orders.Where(item => item.UserID == id).OrderByDescending(item => item.Date);
        }

        // POST: api/Orders/Statuses
        [Route("api/orders/statuses")]
        public IEnumerable<OrderStatus> GetStatuses()
        {
            return db.OrderStatuses;
        }

        // PUT: api/Orders/5
        // Update order status
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.ID)
            {
                return BadRequest();
            }

            //db.Entry(order).State = EntityState.Modified;
            db.Orders.Find(id).StatusID = order.StatusID;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return Ok(db.Entry(order).Entity);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            order.Date = DateTime.Now;
            order.StatusID = 1;
            db.Orders.Add(order);

            foreach (ProductQuantity orderProduct in order.Products)
            {
                Product existingProduct = db.Products.Find(orderProduct.ProductID);
                if (existingProduct != null)
                    if (orderProduct.Quantity > existingProduct.Quantity)
                        return Content(HttpStatusCode.InternalServerError, "We only have " + existingProduct.Quantity + " x '" + existingProduct.Name + "' in stock");
            }

            foreach (ProductQuantity orderProduct in order.Products)
            {
                Product existingProduct = db.Products.Find(orderProduct.ProductID);
                if (existingProduct != null)
                    existingProduct.Quantity -= orderProduct.Quantity;
            }

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.ID }, order);
        }

        // DELETE: api/Orders/5
        //[ResponseType(typeof(Order))]
        //public IHttpActionResult DeleteOrder(int id)
        //{
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Orders.Remove(order);
        //    db.SaveChanges();

        //    return Ok(order);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.ID == id) > 0;
        }
    }
}