namespace ForMen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07UseFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Phone");
        }
    }
}
