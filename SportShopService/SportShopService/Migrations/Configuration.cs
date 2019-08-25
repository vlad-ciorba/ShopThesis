namespace SportShop.Migrations
{
    using SportShop.Models;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SportShop.Models.SportShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SportShop.Models.SportShopContext context)
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
                new User { ID = 1, Username = "admin", Password = "1", FirstName = "Admin", LastName = "SportShop", Email = "admin@SportShop.com", Address = "1 Address", Phone = "0740123456", IsAdmin = true }
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
                new Category { ID = 1, Name = "Palete" },
                new Category { ID = 2, Name = "Mingi" }
            );
            context.Products.AddOrUpdate(
                item => item.Name,
                new Product { ID = 1, Name = "Paleta tenis de masa", CategoryID = 1, Quantity = 15, Description = "Paleta de tenis de masal Adidas KINETIC face parte din produsele de top Adidas. Prin noua tehnologie Kinetic puterea creste pastrand în acelasi timp un foarte bun control. Este special conceputa pentru jocul ofensiv si sprijina jucatori care poseda deja abilitati tehnice respective.", Color = "rosu + albastru", Size = "N/A", Price = 135, DisplayImageID = 1, CreatedDate = DateTime.Now },
                new Product { ID = 2, Name = "Minge fotbal Nike", CategoryID = 2, Quantity = 25, Description = "Design cu multe panouri, Camera de cauciuc la interior pentru retinerea formei, Imprimeu grafic pentru urmarirea mai usoara a mingii, Material: 60% cauciuc, 15% poliuretan, 13% poliester, 12% EVA", Color = "verde", Size = "5", Price = 85, DisplayImageID = 2, CreatedDate = DateTime.Now },
                new Product { ID = 3, Name = "Racheta Babolat Pure Aero", CategoryID = 1, Quantity = 10, Description = "Racheta de tenis Babolat PURE AERO JR 25, destinata juniorilor avansati si celor care participa la competitii. Greutatea redusa o defineste. Model original care ofera un control excelent si confort. Cea mai noua tehnologie Cortex amortizeaza vibratiile nedorite. Cadrul aerodinamic permite jucatorului sa se pregateasca mai rapid pentru lovitura si ofera un spin mai bun.", Color = "negru + verde", Size = "63", Price = 240, DisplayImageID = 3, CreatedDate = DateTime.Now },
                new Product { ID = 4, Name = "Mingi Tenis Babolat First", CategoryID = 2, Quantity = 15, Description = "Mingii de TOP utilizate la cluburile Babolat; Minge Premium - durabilitate si confort; Ideala pentru competitii; Aprobata ITF", Color = "verde", Size = "N/A", Price = 18, DisplayImageID = 4, CreatedDate = DateTime.Now }
            );
            context.Images.AddOrUpdate(
                item => item.URL,
                new Image { ProductID = 1, URL = "https://www.sportguru.ro/media/catalog/product/cache/1/thumbnail/800x615.38461538462/9df78eab33525d08d6e5fb8d27136e95/p/a/paleta-tenis-de-masa-adidas-kinetic.jpg" },
                new Image { ProductID = 2, URL = "https://cdn.sportdepot.bg/files/products/NIKE-SC3933-702_01.jpg" },
                new Image { ProductID = 3, URL = "https://i.sportisimo.com/products/images/855/855138/full/babolat-pure-aero-jr-25_0.jpg" },
                new Image { ProductID = 4, URL = "https://cdn.hervis.ro/medias/sys_master/images/images/h52/h8d/9792908886046/Babolat-First-X-3-1825069-00-47368.jpg" }
            );
            context.Users.AddOrUpdate(
                item => item.Username,
                new User { ID = 2, Username = "vlad", Password = "1", FirstName = "Vlad", LastName = "Admin", Email = "vlad@email.com", Address = "17 Tulcea", Phone = "0740111222", IsAdmin = true },
                new User { ID = 3, Username = "elena", Password = "1", FirstName = "Elena", LastName = "Client", Email = "elena@email.com", Address = "15 Galaxiei", Phone = "0741222333", },
                new User { ID = 4, Username = "alin", Password = "1", FirstName = "Alin", LastName = "Client", Email = "alin@email.com", Address = "74 Primaverii", Phone = "0742333444", },
                new User { ID = 5, Username = "ioana", Password = "1", FirstName = "Ioana", LastName = "Client", Email = "ioana@email.com", Address = "74 Primaverii", Phone = "0743444555", }
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