using AutoMapper;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace Codit.ApiApps.ActiveDirectory.Contracts
{
    public class ContractMapping
    {
        public static void Setup()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<IUser, v1.User>()
                .ForMember(user => user.FirstName, options => options.MapFrom(activeDirectoryUser => activeDirectoryUser.GivenName))
                .ForMember(user => user.LastName, options => options.MapFrom(activeDirectoryUser => activeDirectoryUser.Surname))
            );
        }
    }
}