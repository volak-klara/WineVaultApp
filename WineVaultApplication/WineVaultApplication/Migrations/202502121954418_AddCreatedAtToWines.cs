namespace WineVaultApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedAtToWines : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wines", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Wines", "CreatedAt");
        }
    }
}
