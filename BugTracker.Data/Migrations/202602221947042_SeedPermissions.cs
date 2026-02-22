namespace BugTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedPermissions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntityName = c.String(),
                        EntityId = c.Int(nullable: false),
                        Action = c.String(),
                        Changes = c.String(),
                        UserId = c.Int(),
                        UserName = c.String(),
                        IpAddress = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        PermissionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.PermissionId })
                .ForeignKey("dbo.Permissions", t => t.PermissionId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ApplicationUsers", "RoleId", c => c.Int());
            CreateIndex("dbo.ApplicationUsers", "RoleId");
            AddForeignKey("dbo.ApplicationUsers", "RoleId", "dbo.Roles", "Id");
            DropColumn("dbo.ApplicationUsers", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUsers", "Role", c => c.String());
            DropForeignKey("dbo.ApplicationUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "PermissionId", "dbo.Permissions");
            DropIndex("dbo.RolePermissions", new[] { "PermissionId" });
            DropIndex("dbo.RolePermissions", new[] { "RoleId" });
            DropIndex("dbo.ApplicationUsers", new[] { "RoleId" });
            DropColumn("dbo.ApplicationUsers", "RoleId");
            DropTable("dbo.Permissions");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.Roles");
            DropTable("dbo.AuditLogs");
        }
    }
}
