namespace ForMen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03UserCartOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductQuantities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Cart_ID = c.Int(),
                        Order_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Carts", t => t.Cart_ID)
                .ForeignKey("dbo.Orders", t => t.Order_ID)
                .Index(t => t.Cart_ID)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsAdmin = c.Boolean(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductQuantities", "Order_ID", "dbo.Orders");
            DropForeignKey("dbo.ProductQuantities", "Cart_ID", "dbo.Carts");
            DropIndex("dbo.ProductQuantities", new[] { "Order_ID" });
            DropIndex("dbo.ProductQuantities", new[] { "Cart_ID" });
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.ProductQuantities");
            DropTable("dbo.Carts");
        }
    }
}
