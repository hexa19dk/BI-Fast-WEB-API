namespace BIFastWebAPI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ConfigurationA : DbMigrationsConfiguration<BIFastWebAPI.Data.ApplicationDbContext>
    {
        public ConfigurationA()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BIFastWebAPI.Data.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
