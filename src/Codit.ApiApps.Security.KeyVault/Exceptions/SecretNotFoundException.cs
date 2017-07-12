using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Codit.ApiApps.Common;

namespace Codit.ApiApps.Security.KeyVault.Exceptions
{
    [Serializable]
    public class SecretNotFoundException : Exception
    {
        /// <summary>
        ///     Name of the secret that was not found
        /// </summary>
        public string SecretName { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="secretName">Name of the secret that was not found</param>
        public SecretNotFoundException(string secretName) : base($"Secret '{secretName}' was not found")
        {
            Guard.AgainstNullOrWhitespace(secretName, nameof(secretName));

            SecretName = secretName;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="secretName">Name of the secret that was not found</param>
        /// <param name="inner">Inner exception that is relevant to not finding the secret</param>
        public SecretNotFoundException(string secretName, Exception inner) : base($"Secret '{secretName}' was not found", inner)
        {
            Guard.AgainstNullOrWhitespace(secretName, nameof(secretName));

            SecretName = secretName;
        }

        protected SecretNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            SecretName = info.GetString(nameof(SecretName));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.AgainstNull(info, nameof(info));

            info.AddValue(nameof(SecretName), SecretName);

            base.GetObjectData(info, context);
        }
    }
}
