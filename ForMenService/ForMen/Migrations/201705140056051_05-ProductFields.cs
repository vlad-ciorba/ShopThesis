namespace ForMen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05ProductFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Size", c => c.String());
            AddColumn("dbo.Products", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Color");
            DropColumn("dbo.Products", "Size");
            DropColumn("dbo.Products", "Quantity");
        }
    }
}
