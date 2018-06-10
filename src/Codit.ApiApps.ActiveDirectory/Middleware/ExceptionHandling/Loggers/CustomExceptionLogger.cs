using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Codit.ApiApps.ActiveDirectory.Middleware.DependencyManagement;
using Codit.ApiApps.Common.Telemetry;
using Ninject;

namespace Codit.ApiApps.ActiveDirectory.Middleware.ExceptionHandling.Loggers
{
    public class CustomExceptionLogger : IExceptionLogger
    {
        public virtual Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            LogCore(context);
            return Task.FromResult(0);
        }

        public virtual void LogCore(ExceptionLoggerContext context)
        {
            var customProperties = new Dictionary<string, string>();

            if (context.Request != null)
            {
                var correlationId = context.Request.GetCorrelationId();
                customProperties.Add("CorrelationId", Convert.ToString(correlationId));
            }

            DependencyContainer.Instance.Get<ITelemetry>().TrackException(context.Exception, customProperties);
        }
    }
}