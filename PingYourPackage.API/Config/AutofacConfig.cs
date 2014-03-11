namespace PingYourPackage.API.Config
{
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;

    public class AutofacWebAPI
    {
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config,
            RegisterServices(new ContainerBuilder()));
        }
        public static void Initialize(
        HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver =
            new AutofacWebApiDependencyResolver(container);
        }
        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            // registration goes here
            return builder.Build();
        }
    }
}