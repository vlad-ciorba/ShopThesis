namespace SportShop.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _04OrderStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderStatus",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Code = c.Int(nullable: false),
                    Status = c.String(),
                })
                .PrimaryKey(t => t.ID);

            AddColumn("dbo.Orders", "StatusCode", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Orders", "StatusCode");
            DropTable("dbo.OrderStatus");
        }
    }
}