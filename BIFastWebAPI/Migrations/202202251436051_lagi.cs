namespace BIFastWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lagi : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActivityLogs", "LogDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActivityLogs", "LogDate", c => c.DateTime(nullable: false));
        }
    }
}
