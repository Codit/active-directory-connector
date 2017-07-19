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
        public string JobTitle { get; set; }

        [DataMember]
        public string Department { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string TelephoneNumber { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string UserPrincipalName { get; set; }

        [DataMember]
        public string UserType { get; set; }

        [DataMember]
        public string ObjectId { get; set; }

        [DataMember]
        public string ObjectType { get; set; }

        [DataMember]
        public bool IsAccountEnabled { get; set; }

        /*
         * Groups
         */
    }
}