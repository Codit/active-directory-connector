using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Codit.ApiApps.ActiveDirectory.Repositories
{
    public class ActiveDirectoryRepository
    {
        private static readonly string ServiceUriTemplate = "https://graph.windows.net/{0}";
        private static readonly string AuthorityTemplate = "https://login.microsoftonline.com/{0}";

        protected ActiveDirectoryClient GetActiveDirectoryClient()
        {
            var serviceUri = GetServiceUri();
            var activeDirectoryClient = new ActiveDirectoryClient(serviceUri, GetAccessToken);
            return activeDirectoryClient;
        }

        private static Uri GetServiceUri()
        {
            var activeDirectoryTenant = GetActiveDirectoryTenant();
            var rawServiceUri = string.Format(ServiceUriTemplate, activeDirectoryTenant);
            return new Uri(rawServiceUri);
        }

        private static string GetActiveDirectoryTenant()
        {
            var activeDirectoryTenant = ConfigurationManager.AppSettings.Get("ActiveDirectory.Tenant");
            return activeDirectoryTenant;
        }

        private static async Task<string> GetAccessToken()
        {
            var activeDirectoryTenant = GetActiveDirectoryTenant();
            var authority = string.Format(AuthorityTemplate, activeDirectoryTenant);
            var accessToken = await GetAppToken(authority, "https://graph.windows.net");

            return accessToken;
        }

        private static string GetClientId()
        {
            var clientId = ConfigurationManager.AppSettings.Get("ActiveDirectory.QueryApplication.ClientId");
            return clientId;
        }

        public static async Task<string> GetAppToken(string authority, string resource)
        {
            var clientId = GetClientId();
            var appKey = GetAppKey();
            var authenticationContext = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var clientCredential = new ClientCredential(clientId, appKey);

            var token = await authenticationContext.AcquireTokenAsync(resource, clientCredential);

            return token.AccessToken;
        }

        private static string GetAppKey()
        {
            // TODO: Use Key Vault instead
            var appKey = ConfigurationManager.AppSettings.Get("ActiveDirectory.QueryApplication.AppKey");
            return appKey;
        }
    }
}