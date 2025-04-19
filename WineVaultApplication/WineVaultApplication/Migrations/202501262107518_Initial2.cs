namespace WineVaultApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Wines", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Wines", "UserId");
            AddForeignKey("dbo.Wines", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wines", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Wines", new[] { "UserId" });
            AlterColumn("dbo.Wines", "UserId", c => c.String());
        }
    }
}
