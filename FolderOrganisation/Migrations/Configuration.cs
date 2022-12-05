namespace FolderOrganisation.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FolderOrganisation.DataContext.DatabaseFolder>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "FolderOrganisation.DataContext.DatabaseFolder";
        }

        protected override void Seed(FolderOrganisation.DataContext.DatabaseFolder context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
