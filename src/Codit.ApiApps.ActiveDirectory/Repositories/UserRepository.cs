using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using User = Codit.ApiApps.ActiveDirectory.Contracts.v1.User;

namespace Codit.ApiApps.ActiveDirectory.Repositories
{
    public class UserRepository
    {
        private static readonly string ServiceUriTemplate = "https://graph.windows.net/{0}";
        private static readonly string AuthorityTemplate = "https://login.microsoftonline.com/{0}";

        /// <summary>
        ///     Gets a specific user
        /// </summary>
        /// <param name="objectId">Object Id of the user</param>
        public async Task<User> Get(string objectId)
        {
            var activeDirectoryClient = GetActiveDirectoryClient();
            var foundUser = await activeDirectoryClient.Users.GetByObjectId(objectId).ExecuteAsync();
            var user = MapUserToExternalContract(foundUser);

            return user;
        }

        /// <summary>
        ///     Gets all users
        /// </summary>
        public async Task<List<User>> Get()
        {
            var activeDirectoryClient = GetActiveDirectoryClient();

            var foundUsers = new List<User>();
            var usersPage = await activeDirectoryClient.Users.ExecuteAsync();

            while (usersPage.MorePagesAvailable == false)
            {
                var userNamesInCurrentPage = usersPage.CurrentPage.Select(MapUserToExternalContract);
                foundUsers.AddRange(userNamesInCurrentPage);

                usersPage = await usersPage.GetNextPageAsync();
            }

            return foundUsers;
        }

        private static User MapUserToExternalContract(IUser activeDirectoryUser)
        {
            var user = new User
            {
                FirstName = activeDirectoryUser.GivenName,
                LastName = activeDirectoryUser.Surname,
                DisplayName = activeDirectoryUser.DisplayName,
                UserPrincipalName = activeDirectoryUser.UserPrincipalName,
                CompanyName = activeDirectoryUser.CompanyName,
                Country = activeDirectoryUser.Country,
                Type = activeDirectoryUser.UserType
            };

            return user;
        }

        private static ActiveDirectoryClient GetActiveDirectoryClient()
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