using System;
using System.Threading.Tasks;

namespace Codit.ApiApps.Common.Configuration
{
    public interface ISecretProvider
    {
        /// <summary>
        ///     Retrieves the value for a specific secret
        /// </summary>
        /// <param name="secretName">Name of the secret</param>
        /// <exception cref="ArgumentNullException">Exception thrown when the secret name is not provided, empty or whitespace</exception>
        /// <returns>Value of the secret</returns>
        Task<string> GetSecretAsync(string secretName);
    }
}
