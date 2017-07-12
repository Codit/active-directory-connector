using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Codit.ApiApps.ActiveDirectory.Controllers
{
    [RoutePrefix("api/v1")]
    public class UsersController : ApiController
    {
        [Route("users")]
        public async Task<IHttpActionResult> Get()
        {
            var user = await GetAllUsers();
            return Ok(user);
        }

        private async Task<string> GetAllUsers()
        {
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceUri, GetAccessToken);
            var users = activeDirectoryClient.Users.Take(10);
            return "John Doe";
        }

        private static string clientId = "72a2b371-ad5f-4ce7-a688-e132adeb6260";
        private static string appKey = "GdlG1uyl2BgBs+DPgHrzD7/hZUe9ccAL0sCmHMtn+pQ=";
        private static readonly string rawServiceUri = "https://graph.windows.net/codit.onmicrosoft.com";
        private static readonly Uri serviceUri = new Uri(rawServiceUri);
        private static string authority = "https://login.microsoftonline.com/codit.onmicrosoft.com";

        private static async Task<string> GetAccessToken()
        {
            string accessToken = await GetAppToken(authority, "https://graph.windows.net");

            return accessToken;
        }

        public static async Task<string> GetAppToken(string authority, string resource)
        {
            var authenticationContext = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var clientCredential = new ClientCredential(clientId, appKey);

            var token = await authenticationContext.AcquireTokenAsync(resource, clientCredential);

            return token.AccessToken;
        }
    }
}
