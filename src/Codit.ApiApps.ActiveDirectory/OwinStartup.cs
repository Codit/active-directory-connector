using System.Net.Http.Formatting;
using System.Web.Http;
using Codit.ApiApps.ActiveDirectory;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(OwinStartup))]
namespace Codit.ApiApps.ActiveDirectory
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureRoutes(GlobalConfiguration.Configuration);
            ConfigureFormatters(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        private static void ConfigureRoutes(HttpConfiguration httpConfiguration)
        {
            SwaggerConfig.Register();
            WebApiConfig.Register(httpConfiguration);
        }

        private static void ConfigureFormatters(HttpConfiguration httpConfiguration)
        {
            var jsonFormatter = httpConfiguration.Formatters.JsonFormatter ?? new JsonMediaTypeFormatter();

            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            jsonFormatter.SerializerSettings.Formatting = Formatting.None;
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            httpConfiguration.Formatters.Remove(httpConfiguration.Formatters.JsonFormatter);
            httpConfiguration.Formatters.Insert(0, jsonFormatter);
        }
    }
}