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
                .ForMember(user => user.EmailAddress, options => options.MapFrom(activeDirectoryUser => activeDirectoryUser.Mail))
                .ForMember(user => user.MobilePhoneNumber, options => options.MapFrom(activeDirectoryUser => activeDirectoryUser.Mobile))
                .ForMember(user => user.OfficePhoneNumber, options => options.MapFrom(activeDirectoryUser => activeDirectoryUser.TelephoneNumber))
                .ForMember(user => user.IsAccountEnabled, options => options.MapFrom(activeDirectoryUser => activeDirectoryUser.AccountEnabled))
            );
        }
    }
}