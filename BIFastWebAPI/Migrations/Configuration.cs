namespace BIFastWebAPI.Migrations
{
    using BIFastWebAPI.Data.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BIFastWebAPI.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BIFastWebAPI.Data.ApplicationDbContext context)
        {
            context.ActivityLogs.AddOrUpdate(x => x.Id,
            new ActivityLog() { Channel = "INIT or MIGRATION", EndPoint = "INIT or MIGRATION", BizMsgIdr = "INIT or MIGRATION", OrigTranRefNUM = "INIT or MIGRATION", Type = "INIT or MIGRATION", UserId = "INIT or MIGRATION", ReqMessage = "INIT or MIGRATION", RespMessage = "INIT or MIGRATION", Status = "INIT or MIGRATION", LogDate = DateTime.Now, ReqDate = DateTime.Now, RespDate = DateTime.Now }
            );
        }
    }
}
