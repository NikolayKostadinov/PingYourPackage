namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using PingYourPackage.Domain.Entities;

    public sealed class Configuration : DbMigrationsConfiguration<PingYourPackage.Domain.Entities.Core.EntitiesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PingYourPackage.Domain.Entities.Core.EntitiesContext context)
        {
            context.Roles.AddOrUpdate(role => role.Name,
                new Role { Key = Guid.NewGuid(), Name = "Admin" },
                new Role { Key = Guid.NewGuid(), Name = "Employee" },
                new Role { Key = Guid.NewGuid(), Name = "Affiliate" });
        }
    }
}
