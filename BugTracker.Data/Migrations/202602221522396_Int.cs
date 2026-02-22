namespace BugTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Int : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bugs", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bugs", "CreatedDate");
        }
    }
}
