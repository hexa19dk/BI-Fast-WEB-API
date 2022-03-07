namespace BIFastWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChEpBzNUM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActivityLogs", "EndPoint", c => c.String());
            AddColumn("dbo.ActivityLogs", "BizMsgIdr", c => c.String());
            AddColumn("dbo.ActivityLogs", "OrigTranRefNUM", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActivityLogs", "OrigTranRefNUM");
            DropColumn("dbo.ActivityLogs", "BizMsgIdr");
            DropColumn("dbo.ActivityLogs", "EndPoint");
        }
    }
}
