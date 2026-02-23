namespace BugTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "PasswordHash", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "PasswordHash");
        }
    }
}
