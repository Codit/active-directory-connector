using System;
using System.Diagnostics;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using AutoMapper;
using Codit.ApiApps.ActiveDirectory;
using Codit.ApiApps.ActiveDirectory.Contracts;
using Codit.ApiApps.ActiveDirectory.Contracts.v1;
using Codit.ApiApps.ActiveDirectory.Middleware.ExceptionHandling;
using Codit.ApiApps.ActiveDirectory.Middleware.ExceptionHandling.Handlers;
using Codit.ApiApps.ActiveDirectory.Middleware.ExceptionHandling.Loggers;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using Codit.ApiApps.ActiveDirectory.Middleware.DependencyManagement;
using Codit.ApiApps.Common.Configuration;
using Codit.ApiApps.Common.Telemetry;
using Codit.ApiApps.Security.KeyVault;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Ninject;

[assembly: OwinStartup(typeof(OwinStartup))]
namespace Codit.ApiApps.ActiveDirectory
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                Trace.TraceInformation("Configuring owin.");
                ConfigureDependencyInjection();
                ConfigureExceptionHandling(app, GlobalConfiguration.Configuration);
                ConfigureRoutes(GlobalConfiguration.Configuration);
                ConfigureFormatters(GlobalConfiguration.Configuration);
                ConfigureMapper();

                GlobalConfiguration.Configuration.EnsureInitialized();
                Trace.TraceInformation("Owin configured.");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        private void ConfigureMapper()
        {
            ContractMapping.Setup();
        }

        private static void ConfigureDependencyInjection()
        {
            if (DependencyContainer.Instance.TryGet<ITelemetry>() == null)
            {
                DependencyContainer.Instance.Bind<ITelemetry>().To<ApplicationInsightsTelemetry>().InSingletonScope();
            }

            if (DependencyContainer.Instance.TryGet<ISecretProvider>() == null)
            {
                DependencyContainer.Instance.Bind<ISecretProvider>().To<KeyVaultSecretProvider>().InSingletonScope();
            }

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver();
        }

        private static void ConfigureRoutes(HttpConfiguration httpConfiguration)
        {
            SwaggerConfig.Register();
            WebApiConfig.Register(httpConfiguration);
        }

        private static void ConfigureExceptionHandling(IAppBuilder app, HttpConfiguration httpConfiguration)
        {
            app.Use<GlobalExceptionMiddleware>();
            httpConfiguration.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionResponseHandler());
            httpConfiguration.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger());
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