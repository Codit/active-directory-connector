using System.Runtime.Serialization;

namespace Codit.ApiApps.ActiveDirectory.Contracts.v1
{
    [DataContract]
    public class User : IContract
    {
        [DataMember]
        public int Version { get; } = 1;

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string UserPrincipalName { get; set; }

        [DataMember]
        public bool IsAccountEnabled { get; set; }

        [DataMember]
        public ContactInformation ContactInformation { get; set; }

        [DataMember]
        public CompanyInformation CompanyInformation { get; set; }

        [DataMember]
        public UserMetadata Metadata { get; set; }
    }
}