namespace BIFastWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Channel = c.String(),
                        Type = c.String(),
                        UserId = c.String(),
                        ReqMessage = c.String(),
                        RespMessage = c.String(),
                        Status = c.String(),
                        LogDate = c.DateTime(nullable: false),
                        ReqDate = c.DateTime(nullable: false),
                        RespDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ActivityLogs");
        }
    }
}
