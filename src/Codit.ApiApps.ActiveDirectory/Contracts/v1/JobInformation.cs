using System.Runtime.Serialization;

namespace Codit.ApiApps.ActiveDirectory.Contracts.v1
{
    [DataContract]
    public class JobInformation
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Department { get; set; }

        [DataMember]
        public string CompanyName { get; set; }
    }
}