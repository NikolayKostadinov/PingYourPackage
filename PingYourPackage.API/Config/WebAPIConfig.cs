namespace PingYourPackage.API.Config
{
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.Validation;
    using System.Web.Http.Validation.Providers;
    using PingYourPackage.API.Formatting;
    using PingYourPackage.API.MessageHandlers;
    using PingYourPackage.API.Model.RequestCommands;
    using WebApiDoodle.Web.Filters;

    public class WebAPIConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            //Message Handlers
            config.MessageHandlers.Add(new RequireHttpsMessageHandler());
            config.MessageHandlers.Add(new PingYourPackageAuthHandler());

            //Formatters
            var jqueryFormatter = config.Formatters.FirstOrDefault(x => x.GetType() == typeof(JQueryMvcFormUrlEncodedFormatter));

            config.Formatters.Remove(config.Formatters.FormUrlEncodedFormatter);

            config.Formatters.Remove(jqueryFormatter);

            foreach (var formatter in config.Formatters)
            {
                formatter.RequiredMemberSelector = new SuppressedRequiredMemberSelector();
            }

            //Default Services
            config.Services.Replace(
                typeof(IContentNegotiator),
                new DefaultContentNegotiator(excludeMatchOnTypeOnly: true));

            config.Services.RemoveAll(
                typeof(ModelValidatorProvider),
                validator => !(validator is DataAnnotationsModelValidatorProvider));

            // Filters
            config.Filters.Add(new InvalidModelStateFilterAttribute());

            // ParameterBindingRules

            // Any complex type parameter which is Assignable From
            // IRequestCommand will be bound from the URI
            config.ParameterBindingRules.Insert(0, descriptor =>
                typeof(IRequestCommand).IsAssignableFrom(descriptor.ParameterType)
                    ? new FromUriAttribute().GetBinding(descriptor) : null);
        }
    }
}