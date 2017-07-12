using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Codit.ApiApps.ActiveDirectory.Middleware.ExceptionHandling.Handlers
{
    public class GlobalExceptionResponseHandler : ExceptionHandler
    {
        public override async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                "The request could not be completed, please try again.");
            var webException = context.Exception as WebException;
            if (webException != null)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                var statusMessage = "The request could not be completed, please try again.";
                if (webException.Status == WebExceptionStatus.Timeout)
                    statusCode = HttpStatusCode.RequestTimeout;
                if (webException.Status == WebExceptionStatus.ConnectFailure ||
                    webException.Status == WebExceptionStatus.ProxyNameResolutionFailure ||
                    webException.Status == WebExceptionStatus.ReceiveFailure)
                    statusCode = HttpStatusCode.BadGateway;
                if (webException.Status == WebExceptionStatus.ProtocolError)
                {
                    statusCode = HttpStatusCode.Forbidden;
                    statusMessage =
                        "The request could not be completed. Check that you are correctly authenticated and have rights to access the resource.";
                }
                response = context.Request.CreateResponse(statusCode, statusMessage);
            }

            await Task.FromResult(context.Result = new ResponseMessageResult(response));
        }
    }
}