using System;
using System.Configuration;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using Codit.ApiApps.Common;
using Codit.ApiApps.Common.Configuration;
using Codit.ApiApps.Security.KeyVault.Exceptions;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Codit.ApiApps.Security.KeyVault
{
    public class KeyVaultSecretProvider : ISecretProvider
    {
        private const string UriConfigurationName = "KeyVault.Uri";
        private const string ClientIdConfigurationName = "KeyVault.ClientId";
        private const string ClientSecretConfigurationName = "KeyVault.ClientSecret";

        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(1)
        };

        /// <summary>
        ///     Uri to the Azure Key Vault that is being consulted
        /// </summary>
        public string VaultUri => ConfigurationManager.AppSettings.Get(UriConfigurationName);

        /// <summary>
        ///     Retrieves the value for a specific secret in Azure Key Vault
        /// </summary>
        /// <param name="secretName">Name of the secret</param>
        /// <exception cref="ArgumentNullException">Exception thrown when the secret name is not provided, empty or whitespace</exception>
        /// <exception cref="ArgumentException">Exception thrown when the secret name contains a '.'</exception>
        /// <returns>Value of the secret</returns>
        public virtual async Task<string> GetSecretAsync(string secretName)
        {
            ValidateSecretName(secretName);

            try
            {
                var keyVaultClient = new KeyVaultClient(KeyVaultAuthenticationFallbackAsync, _httpClient);
                var secretValue = await keyVaultClient.GetSecretAsync(VaultUri, secretName);

                return secretValue.Value;
            }
            catch (KeyVaultErrorException ex)
            {
                switch (ex.Body?.Error.Code.ToLower())
                {
                    case "secretnotfound":
                        throw new SecretNotFoundException(secretName, ex);
                    case "forbidden":
                        throw new UnauthorizedAccessException($"Unauthorized to access secret '{secretName}'.", ex);
                    default:
                        throw;
                }
            }
        }

        protected void ValidateSecretName(string secretName)
        {
            Guard.AgainstNullOrWhitespace(secretName, nameof(secretName));

            if (secretName.Contains("."))
            {
                throw new ArgumentException($"Secret name '{secretName}' contains a '.'");
            }
        }

        private async Task<string> KeyVaultAuthenticationFallbackAsync(string authority, string resource, string scope)
        {
            var authenticationContext = new AuthenticationContext(authority, TokenCache.DefaultShared);

            var clientId = ConfigurationManager.AppSettings.Get(ClientIdConfigurationName);
            var clientSecret = GetClientSecret();
            var secureClientSecret = new SecureClientSecret(clientSecret);

            var clientCredential = new ClientCredential(clientId, secureClientSecret);
            var authenticationToken = await authenticationContext.AcquireTokenAsync(resource, clientCredential);
            if (authenticationToken == null)
            {
                throw new UnauthorizedAccessException("Unable to get an authentication token");
            }

            return authenticationToken.AccessToken;
        }

        private SecureString GetClientSecret()
        {
            var securedClientSecret = new SecureString();
            var clientSecret = ConfigurationManager.AppSettings.Get(ClientSecretConfigurationName);
            foreach (var character in clientSecret)
            {
                securedClientSecret.AppendChar(character);
            }

            return securedClientSecret;
        }
    }
}
