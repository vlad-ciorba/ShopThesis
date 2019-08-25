namespace SportShop.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _02Image : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ProductID = c.Int(nullable: false),
                    URL = c.String(),
                })
                .PrimaryKey(t => t.ID);

            AddColumn("dbo.Products", "DisplayImageID", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Products", "DisplayImageID");
            DropTable("dbo.Images");
        }
    }
}