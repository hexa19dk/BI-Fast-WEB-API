namespace BIFastWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddbankMaster : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankMasters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KodeBank = c.String(),
                        NamaBank = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BankMasters");
        }
    }
}
