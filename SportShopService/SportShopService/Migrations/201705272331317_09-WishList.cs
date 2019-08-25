namespace SportShop.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _09WishList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishLists",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    UserID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);

            AddColumn("dbo.ProductQuantities", "WishList_ID", c => c.Int());
            CreateIndex("dbo.ProductQuantities", "WishList_ID");
            AddForeignKey("dbo.ProductQuantities", "WishList_ID", "dbo.WishLists", "ID");
        }

        public override void Down()
        {
            DropForeignKey("dbo.WishLists", "UserID", "dbo.Users");
            DropForeignKey("dbo.ProductQuantities", "WishList_ID", "dbo.WishLists");
            DropIndex("dbo.WishLists", new[] { "UserID" });
            DropIndex("dbo.ProductQuantities", new[] { "WishList_ID" });
            DropColumn("dbo.ProductQuantities", "WishList_ID");
            DropTable("dbo.WishLists");
        }
    }
}