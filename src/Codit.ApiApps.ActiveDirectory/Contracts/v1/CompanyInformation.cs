using System.Runtime.Serialization;

namespace Codit.ApiApps.ActiveDirectory.Contracts.v1
{
    [DataContract]
    public class CompanyInformation
    {
        [DataMember]
        public string JobTitle { get; set; }

        [DataMember]
        public string Department { get; set; }

        [DataMember]
        public string CompanyName { get; set; }
    }
}