namespace ForMen.Migrations
{
    using ForMen.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ForMen.Models.ForMenContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ForMen.Models.ForMenContext context)
        {
            context.OrderStatuses.AddOrUpdate(
                item => item.Status,
                new OrderStatus { ID = 1, Status = "Processing" },
                new OrderStatus { ID = 2, Status = "Cancelled" },
                new OrderStatus { ID = 3, Status = "Delivered" },
                new OrderStatus { ID = 4, Status = "Received" },
                new OrderStatus { ID = 5, Status = "Returned" }
            );

            context.Users.AddOrUpdate(
                item => item.Username,
                new User { ID = 1, Username = "admin", Password = "admin", FirstName = "Admin", LastName = "Admin", Email = "admin@formenwebshop.com", Address = "1 Address", Phone = "0740123456", IsAdmin = true }
            );
            context.Carts.AddOrUpdate(
                item => item.UserID,
                new Cart { ID = 1, UserID = 1 }
            );
            context.WishLists.AddOrUpdate(
                item => item.UserID,
                new WishList { ID = 1, UserID = 1 }
            );

            ///// FOR DEVELOPEMENT /////
            context.Categories.AddOrUpdate(
                item => item.Name,
                new Category { ID = 1, Name = "Imbracaminte" },
                new Category { ID = 2, Name = "Incaltaminte" }
            );
            context.Products.AddOrUpdate(
                item => item.Name,
                new Product { ID = 1, Name = "Camasa", CategoryID = 1, Quantity = 10, Description = "Camasa neagra", Color = "negru", Size = "M,L,XL", Price = 100, DisplayImageID = 1, CreatedDate = DateTime.Now },
                new Product { ID = 2, Name = "Pantaloni", CategoryID = 1, Quantity = 10, Description = "Pantaloni albastri", Color = "albastru", Size = "M", Price = 120, DisplayImageID = 2, CreatedDate = DateTime.Now },
                new Product { ID = 3, Name = "Pantofi", CategoryID = 2, Quantity = 10, Description = "Pantofi negri", Color = "negru", Size = "42", Price = 180, DisplayImageID = 3, CreatedDate = DateTime.Now },
                new Product { ID = 4, Name = "Tenisi", CategoryID = 2, Quantity = 10, Description = "Tenisi rosi", Color = "rosu", Size = "42", Price = 80, DisplayImageID = 4, CreatedDate = DateTime.Now }
            );
            context.Images.AddOrUpdate(
                item => item.URL,
                new Image { ProductID = 1, URL = "https://images.okr.ro/serve/auctions.v7/2016/feb/29/9c9dc2e916655d9fbdf7f99c2dba80f7-0-235_235_11.jpg" },
                new Image { ProductID = 2, URL = "https://s-media-cache-ak0.pinimg.com/236x/aa/ed/d3/aaedd36a0939e101f509f3bc2e9847c2.jpg" },
                new Image { ProductID = 3, URL = "https://www.don-men.com/media/catalog/product/cache/1/small_image/400x/9df78eab33525d08d6e5fb8d27136e95/p/a/pantofi_barbati-eleganti-piele-donmen-louis_2_.jpg" },
                new Image { ProductID = 4, URL = "http://exclusivista.ro/wp-content/uploads/2014/05/Tenisi-rosii-Converse-All-Star-OX-pentru-femei-M9696_tenisi-unisex-converse-all-star-ox-m9696-9412-1_500_500-300x300.jpg" }
            );
            context.Users.AddOrUpdate(
                item => item.Username,
                new User { ID = 2, Username = "vlad", Password = "1", FirstName = "Vlad", LastName = "Dev", Email = "vlad@email.com", Address = "17 Tulcea", Phone = "0740111222", IsAdmin = true },
                new User { ID = 3, Username = "elena", Password = "1", FirstName = "Elena", LastName = "Dev", Email = "elena@email.com", Address = "15 Galaxiei", Phone = "0741222333", },
                new User { ID = 4, Username = "alin", Password = "1", FirstName = "Alin", LastName = "Dev", Email = "alin@email.com", Address = "74A Porlissum", Phone = "0742333444", },
                new User { ID = 5, Username = "ioana", Password = "1", FirstName = "Ioana", LastName = "Dev", Email = "ioana@email.com", Address = "74A Porlissum", Phone = "0743444555", }
            );
            context.Carts.AddOrUpdate(
                item => item.UserID,
                new Cart { ID = 2, UserID = 2 },
                new Cart { ID = 3, UserID = 3 },
                new Cart { ID = 4, UserID = 4 },
                new Cart { ID = 5, UserID = 5 }
            );
            context.WishLists.AddOrUpdate(
                item => item.UserID,
                new WishList { ID = 2, UserID = 2 },
                new WishList { ID = 3, UserID = 3 },
                new WishList { ID = 4, UserID = 4 },
                new WishList { ID = 5, UserID = 5 }
            );
        }
    }
}
