using System;
using System.Threading.Tasks;
using Codit.ApiApps.Common.Telemetry;
using Microsoft.Owin;

namespace Codit.ApiApps.ActiveDirectory.Middleware.ExceptionHandling
{
    public class GlobalExceptionMiddleware : OwinMiddleware
    {
        private readonly ITelemetry _telemetry;

        public GlobalExceptionMiddleware(OwinMiddleware next, ITelemetry telemetry) : base(next)
        {
            _telemetry = telemetry;
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception exception)
            {
                _telemetry.TrackException(exception);
                throw;
            }
        }
    }
}