namespace BIFastWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteBaskList : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.BankMasters");
        }
        
        public override void Down()
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
    }
}
