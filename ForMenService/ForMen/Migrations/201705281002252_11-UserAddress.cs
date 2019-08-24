namespace ForMen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11UserAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Address");
        }
    }
}
