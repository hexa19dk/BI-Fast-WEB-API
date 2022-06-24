namespace BIFastWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegistrationDataTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RegistrationDatas", "NoRek", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RegistrationDatas", "NoRek");
        }
    }
}
