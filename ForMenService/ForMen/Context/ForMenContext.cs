using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ForMen.Models
{
    public class ForMenContext : DbContext
    {
        public ForMenContext() : base("name=ForMenContext")
        {
        }

        public System.Data.Entity.DbSet<ForMen.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<ForMen.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<ForMen.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<ForMen.Models.Image> Images { get; set; }

        public System.Data.Entity.DbSet<ForMen.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<ForMen.Models.Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<ForMen.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<ForMen.Models.ProductQuantity> ProductQuantities { get; set; }

        public System.Data.Entity.DbSet<ForMen.Models.OrderStatus> OrderStatuses { get; set; }

        public System.Data.Entity.DbSet<ForMen.Models.WishList> WishLists { get; set; }

        //public System.Data.Entity.DbSet<ForMen.Models.Contact> Contacts { get; set; }
    }
}
