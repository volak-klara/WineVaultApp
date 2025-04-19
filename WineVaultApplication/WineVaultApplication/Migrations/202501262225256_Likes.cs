namespace WineVaultApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Likes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WineLikes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WineId = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WineLikes");
        }
    }
}
