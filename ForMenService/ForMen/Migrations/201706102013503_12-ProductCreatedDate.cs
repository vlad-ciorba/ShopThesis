namespace ForMen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12ProductCreatedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "CreatedDate");
        }
    }
}
