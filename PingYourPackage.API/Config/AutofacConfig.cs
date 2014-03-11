namespace PingYourPackage.API.Config
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;

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

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());



            return builder.Build();
        }
    }

}