using SportShop.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace SportShop.Controllers
{
    public class ImagesController : ApiController
    {
        private SportShopContext db = new SportShopContext();

        // GET: api/Images
        public IQueryable<Image> GetImages()
        {
            return db.Images;
        }

        // GET: api/Images/5
        [ResponseType(typeof(Image))]
        public IHttpActionResult GetImage(int id)
        {
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        // PUT: api/Images/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutImage(int id, Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != image.ID)
            {
                return BadRequest();
            }

            db.Entry(image).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(db.Entry(image).Entity);
        }

        public string UploadImage(string base64String, int ProductID)
        {
            byte[] bytes = Convert.FromBase64String(base64String);

            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms);
            }

            string imageLocation = "files/images/" + ProductID + ".png";
            string localPath = AppDomain.CurrentDomain.BaseDirectory + imageLocation;
            File.WriteAllBytes(localPath, Convert.FromBase64String(base64String));

            return Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/" + imageLocation;
        }

        // POST: api/Images
        [ResponseType(typeof(Image))]
        public IHttpActionResult PostImage(ImageRequest image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Image img = new Image()
            {
                ProductID = image.ProductID,
                URL = UploadImage(image.Base64EncodedImage, image.ProductID)
            };

            db.Images.Add(img);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = img.ID }, img);
        }

        // DELETE: api/Images/5
        [ResponseType(typeof(Image))]
        public IHttpActionResult DeleteImage(int id)
        {
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return NotFound();
            }

            db.Images.Remove(image);
            db.SaveChanges();

            return Ok(image);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImageExists(int id)
        {
            return db.Images.Count(e => e.ID == id) > 0;
        }
    }
}