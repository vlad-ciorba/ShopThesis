using SportShop.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace SportShop.Controllers
{
    public class WishListsController : ApiController
    {
        private SportShopContext db = new SportShopContext();

        // GET: api/WishLists
        //public IQueryable<WishList> GetWishLists()
        //{
        //    return db.WishLists;
        //}

        //GET: api/WishLists/5
        public WishList GetWishList(int id)
        {
            return db.WishLists.Where(item => item.UserID == id).FirstOrDefault();
        }

        // PUT: api/WishLists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWishList(int id, WishList wishList)
        {
            WishList dbWishList = db.WishLists.Find(id);
            if (wishList == null)
            {
                return NotFound();
            }

            List<ProductQuantity> dbWishListProducts = dbWishList.Products.Select(item => item).ToList();

            foreach (ProductQuantity wishListProduct in wishList.Products)
            {
                foreach (ProductQuantity dbWishListProduct in dbWishListProducts)
                {
                    if (wishListProduct.ProductID == dbWishListProduct.ProductID)
                    {
                        db.ProductQuantities.Remove(dbWishListProduct);
                    }
                }
            }

            //db.WishLists.Remove(wishList);
            db.SaveChanges();

            return Ok(wishList);
        }

        // POST: api/WishLists
        [ResponseType(typeof(WishList))]
        public IHttpActionResult PostWishList(WishList wishList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WishList userWishList = db.WishLists.Where(item => item.UserID == wishList.UserID).FirstOrDefault();

            foreach (ProductQuantity wishListProduct in wishList.Products)
            {
                bool exists = false;

                foreach (ProductQuantity userWishListProduct in userWishList.Products)
                {
                    if (userWishListProduct.ProductID == wishListProduct.ProductID)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    userWishList.Products.Add(wishListProduct);
            }

            db.SaveChanges();
            return Ok(userWishList);
            //return CreatedAtRoute("DefaultApi", new { id = wishList.ID }, wishList);
        }

        // DELETE: api/WishLists
        [ResponseType(typeof(WishList))]
        public IHttpActionResult DeleteWishList(int id)
        {
            WishList wishList = db.WishLists.Find(id);
            if (wishList == null)
            {
                return NotFound();
            }

            List<ProductQuantity> wishListProducts = wishList.Products.Select(item => item).ToList();

            foreach (ProductQuantity wishListProduct in wishListProducts)
            {
                db.ProductQuantities.Remove(wishListProduct);
            }

            //db.WishLists.Remove(wishList);
            db.SaveChanges();

            return Ok(wishList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WishListExists(int id)
        {
            return db.WishLists.Count(e => e.ID == id) > 0;
        }
    }
}