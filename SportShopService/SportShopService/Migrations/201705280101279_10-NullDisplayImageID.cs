namespace SportShop.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _10NullDisplayImageID : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "DisplayImageID", c => c.Int());
        }

        public override void Down()
        {
            AlterColumn("dbo.Products", "DisplayImageID", c => c.Int(nullable: false));
        }
    }
}