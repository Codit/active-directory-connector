using System.Runtime.Serialization;

namespace Codit.ApiApps.ActiveDirectory.Contracts.v1
{
    [DataContract]
    public class UserMetadata
    {
        [DataMember]
        public string UserType { get; set; }

        [DataMember]
        public string ObjectId { get; set; }

        [DataMember]
        public string ObjectType { get; set; }
    }
}