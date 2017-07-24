using System.Runtime.Serialization;

namespace Codit.ApiApps.ActiveDirectory.Contracts.v1
{
    [DataContract]
    public class ContactInformation
    {
        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string OfficePhoneNumber { get; set; }

        [DataMember]
        public string MobilePhoneNumber { get; set; }
    }
}