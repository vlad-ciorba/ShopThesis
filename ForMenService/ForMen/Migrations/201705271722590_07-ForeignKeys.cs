namespace ForMen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07ForeignKeys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "StatusID", c => c.Int(nullable: false));
            AlterColumn("dbo.Categories", "ParentID", c => c.Int());
            CreateIndex("dbo.Carts", "UserID");
            CreateIndex("dbo.ProductQuantities", "ProductID");
            CreateIndex("dbo.Products", "CategoryID");
            CreateIndex("dbo.Categories", "ParentID");
            CreateIndex("dbo.Comments", "ProductID");
            CreateIndex("dbo.Images", "ProductID");
            CreateIndex("dbo.Orders", "UserID");
            CreateIndex("dbo.Orders", "StatusID");
            AddForeignKey("dbo.Categories", "ParentID", "dbo.Categories", "ID");
            AddForeignKey("dbo.Products", "CategoryID", "dbo.Categories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProductQuantities", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Carts", "UserID", "dbo.Users", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Images", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "StatusID", "dbo.OrderStatus", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "UserID", "dbo.Users", "ID", cascadeDelete: true);
            DropColumn("dbo.Orders", "StatusCode");
            DropColumn("dbo.OrderStatus", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderStatus", "Code", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "StatusCode", c => c.Int(nullable: false));
            DropForeignKey("dbo.Orders", "UserID", "dbo.Users");
            DropForeignKey("dbo.Orders", "StatusID", "dbo.OrderStatus");
            DropForeignKey("dbo.Images", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Comments", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Carts", "UserID", "dbo.Users");
            DropForeignKey("dbo.ProductQuantities", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Categories", "ParentID", "dbo.Categories");
            DropIndex("dbo.Orders", new[] { "StatusID" });
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropIndex("dbo.Images", new[] { "ProductID" });
            DropIndex("dbo.Comments", new[] { "ProductID" });
            DropIndex("dbo.Categories", new[] { "ParentID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.ProductQuantities", new[] { "ProductID" });
            DropIndex("dbo.Carts", new[] { "UserID" });
            AlterColumn("dbo.Categories", "ParentID", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "StatusID");
        }
    }
}
