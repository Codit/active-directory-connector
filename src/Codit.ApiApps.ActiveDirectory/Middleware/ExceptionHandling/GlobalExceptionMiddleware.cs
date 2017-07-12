using System;
using System.Threading.Tasks;
using Codit.ApiApps.ActiveDirectory.Middleware.DependencyManagement;
using Codit.ApiApps.Common.Telemetry;
using Microsoft.Owin;
using Ninject;

namespace Codit.ApiApps.ActiveDirectory.Middleware.ExceptionHandling
{
    public class GlobalExceptionMiddleware : OwinMiddleware
    {
        public GlobalExceptionMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception exception)
            {
                DependencyContainer.Instance.Get<ITelemetry>().TrackException(exception);
                throw;
            }
        }
    }
}