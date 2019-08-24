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
using System.Net.Mail;

namespace ForMen.Controllers
{
    public class UsersController : ApiController
    {
        private ForMenContext db = new ForMenContext();

        // POST: api/Users/Login
        [Route("api/users/login")]
        public IHttpActionResult Login(User user)
        {
            User dbUser = db.Users.Where(item => item.Username == user.Username && item.Password == user.Password).FirstOrDefault();

            if (dbUser != null)
                return Ok(mapUser(dbUser));
            else
                return Content(HttpStatusCode.Unauthorized, "Invalid credentials");
        }

        // POST: api/Users/Recover
        [Route("api/users/recover")]
        public IHttpActionResult Recover(User user)
        {
            User dbUser = db.Users.Where(item => item.Email == user.Email).FirstOrDefault();

            if (dbUser != null)
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("formenwebshop@gmail.com");
                msg.To.Add(dbUser.Email);
                msg.Subject = "ForMen password reset";
                msg.Body = "Hi\r\nYour credentials on ForMen web shop are:\r\n\r\nUsername: " + dbUser.Username + "\r\nPassword: " + dbUser.Password;
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("formenwebshop@gmail.com", "pufuleti");
                client.Timeout = 3000;
                try
                {
                    client.Send(msg);
                }
                catch
                {
                    return Content(HttpStatusCode.Unauthorized, "Failed to send mail. Please try again.");
                }
            }
            else
            {
                System.Threading.Thread.Sleep(2000);
            }

            return Ok();
        }

        // GET: api/Users
        public IEnumerable<ResponseUser> GetUsers()
        {
            foreach (User user in db.Users)
            {
                yield return mapUser(user);
            }
        }

        // GET: api/Users/5
        [ResponseType(typeof(ResponseUser))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(mapUser(user));
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }

            User dbUser = db.Users.Where(item => item.Username == user.Username && item.ID != user.ID).FirstOrDefault();
            if (dbUser != null)
                return Content(HttpStatusCode.InternalServerError, "Username already taken");
            dbUser = db.Users.Where(item => item.Email == user.Email && item.ID != user.ID).FirstOrDefault();
            if (dbUser != null)
                return Content(HttpStatusCode.InternalServerError, "Email already taken");

            db.Entry(user).State = EntityState.Modified;

            if (user.Password == null || user.Password.Length == 0) // if password not changed
            {
                db.Entry(user).Property(item => item.Password).IsModified = false;
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(db.Entry(user).Entity);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User dbUser = db.Users.Where(item => item.Username == user.Username).FirstOrDefault();
            if (dbUser != null)
                return Content(HttpStatusCode.InternalServerError, "Username already taken");
            dbUser = db.Users.Where(item => item.Email == user.Email).FirstOrDefault();
            if (dbUser != null)
                return Content(HttpStatusCode.InternalServerError, "Email already taken");

            db.Users.Add(user);
            db.SaveChanges();
            db.Entry(user).GetDatabaseValues();
            db.Carts.Add(new Cart { UserID = user.ID });
            db.WishLists.Add(new WishList { UserID = user.ID });
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.ID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            // Delete user cart
            Cart cart = db.Carts.Where(item => item.UserID == id).FirstOrDefault();
            List<ProductQuantity> cartProducts = cart.Products.Select(item => item).ToList();

            foreach (ProductQuantity cartProduct in cartProducts)
            {
                db.ProductQuantities.Remove(cartProduct);
            }

            // Delete user orders
            List<Order> orders = db.Orders.Where(item => item.UserID == id).ToList();
            List<ProductQuantity> orderProducts = new List<ProductQuantity>();

            foreach (Order order in orders)
            {
                foreach (ProductQuantity orderProduct in order.Products)
                {
                    orderProducts.Add(orderProduct);
                }
            }
            foreach (ProductQuantity orderProduct in orderProducts)
            {
                db.ProductQuantities.Remove(orderProduct);
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.ID == id) > 0;
        }

        private ResponseUser mapUser(User user)
        {
            return new ResponseUser
            {
                ID = user.ID,
                IsAdmin = user.IsAdmin,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                Username = user.Username
            };
        }
    }
}