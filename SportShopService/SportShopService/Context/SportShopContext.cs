using System.Data.Entity;

namespace SportShop.Models
{
    public class SportShopContext : DbContext
    {
        public SportShopContext() : base("name=SportShopContext")
        {
        }

        public System.Data.Entity.DbSet<SportShop.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<SportShop.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<SportShop.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<SportShop.Models.Image> Images { get; set; }

        public System.Data.Entity.DbSet<SportShop.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<SportShop.Models.Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<SportShop.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<SportShop.Models.ProductQuantity> ProductQuantities { get; set; }

        public System.Data.Entity.DbSet<SportShop.Models.OrderStatus> OrderStatuses { get; set; }

        public System.Data.Entity.DbSet<SportShop.Models.WishList> WishLists { get; set; }

        //public System.Data.Entity.DbSet<SportShop.Models.Contact> Contacts { get; set; }
    }
}