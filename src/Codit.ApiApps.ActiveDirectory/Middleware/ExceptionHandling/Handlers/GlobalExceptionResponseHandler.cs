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

            if (context.Exception is WebException webException)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                var statusMessage = "The request could not be completed, please try again.";

                switch (webException.Status)
                {
                    case WebExceptionStatus.Timeout:
                        statusCode = HttpStatusCode.RequestTimeout;
                        break;
                    case WebExceptionStatus.ConnectFailure:
                    case WebExceptionStatus.ProxyNameResolutionFailure:
                    case WebExceptionStatus.ReceiveFailure:
                        statusCode = HttpStatusCode.BadGateway;
                        break;
                    case WebExceptionStatus.ProtocolError:
                        statusCode = HttpStatusCode.Forbidden;
                        statusMessage =
                            "The request could not be completed. Check that you are correctly authenticated and have rights to access the resource.";
                        break;
                }

                response = context.Request.CreateResponse(statusCode, statusMessage);
            }

            await Task.FromResult(context.Result = new ResponseMessageResult(response));
        }
    }
}