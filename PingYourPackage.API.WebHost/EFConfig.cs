namespace PingYourPackage.API.WebHost
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public class EFConfig
    {
        public static void Initialize()
        {
            RunMigrations();
        }

        private static void RunMigrations()
        {
            var efMigrationSettings = new PingYourPackage.Domain.Migrations.Configuration();
            var efMigrator = new DbMigrator(efMigrationSettings);

            efMigrator.Update();
        }
    }
}