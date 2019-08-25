using SportShop.Models;
using System.Net;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;

namespace SportShop.Controllers
{
    public class ContactsController : ApiController
    {
        private SportShopContext db = new SportShopContext();

        //// GET: api/Contacts
        //public IQueryable<Contact> GetContacts()
        //{
        //    return db.Contacts;
        //}

        //// GET: api/Contacts/5
        //[ResponseType(typeof(Contact))]
        //public IHttpActionResult GetContact(int id)
        //{
        //    Contact contact = db.Contacts.Find(id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(contact);
        //}

        //// PUT: api/Contacts/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutContact(int id, Contact contact)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != contact.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(contact).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ContactExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Contacts
        [ResponseType(typeof(Contact))]
        public IHttpActionResult PostContact(Contact contact)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("SportShop.WebShop@gmail.com");
            msg.To.Add("eth_vlad@yahoo.com");
            msg.Subject = "SportShop contact";
            msg.ReplyToList.Add(contact.Email);
            msg.Body = "Hello from SportShop,\r\n" + contact.FullName + " (" + contact.Email + " / " + contact.Phone + ") contacted you for:\r\n\r\n" + contact.Message;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("SportShop.WebShop@gmail.com", "pufuleti");
            client.Timeout = 3000;
            try
            {
                client.Send(msg);
            }
            catch
            {
            }

            return Ok();
        }

        // DELETE: api/Contacts/5
        //[ResponseType(typeof(Contact))]
        //public IHttpActionResult DeleteContact(int id)
        //{
        //    Contact contact = db.Contacts.Find(id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Contacts.Remove(contact);
        //    db.SaveChanges();

        //    return Ok(contact);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool ContactExists(int id)
        //{
        //    return db.Contacts.Count(e => e.ID == id) > 0;
        //}
    }
}