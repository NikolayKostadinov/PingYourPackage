namespace PingYourPackage.API.Config
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using PingYourPackage.Domain.Entities.Core;
    using PingYourPackage.Domain.Services;

    public class AutofacWebAPI
    {

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(
                Assembly.GetExecutingAssembly())
                .PropertiesAutowired();

            //EF DbContext
            builder.RegisterType<EntitiesContext>()
                .As<DbContext>()
                .InstancePerApiRequest();

            //Repositories
            builder.RegisterGeneric(typeof(EntityRepository<>))
                .As(typeof(IEntityRepository<>))
                .InstancePerApiRequest();

            //Services
            builder.RegisterType<CryptoService>()
                .As<ICryptoService>()
                .InstancePerApiRequest();

            builder.RegisterType<MembershipService>()
                .As<IMembershipService>()
                .InstancePerApiRequest();

            builder.RegisterType<ShipmentService>()
                .As<IShipmentService>()
                .InstancePerApiRequest();

            return builder.Build();
        }

    }

}