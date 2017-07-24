using AutoMapper;
using Codit.ApiApps.ActiveDirectory.Contracts.v1;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using User = Codit.ApiApps.ActiveDirectory.Contracts.v1.User;

namespace Codit.ApiApps.ActiveDirectory.Contracts
{
    public class ContractMapping
    {
        public static void Setup()
        {
            Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<IUser, CompanyInformation>();
                    cfg.CreateMap<IUser, ContactInformation>()
                        .ForMember(contactInformation => contactInformation.EmailAddress, opt => opt.MapFrom(src => src.Mail))
                        .ForMember(contactInformation => contactInformation.MobilePhoneNumber, opt => opt.MapFrom(src => src.Mobile))
                        .ForMember(contactInformation => contactInformation.OfficePhoneNumber, opt => opt.MapFrom(src => src.TelephoneNumber));
                    cfg.CreateMap<IUser, UserMetadata>();
                    cfg.CreateMap<IUser, User>()
                        .ForMember(user => user.FirstName, opt => opt.MapFrom(src => src.GivenName))
                        .ForMember(user => user.LastName, opt => opt.MapFrom(src => src.Surname))
                        .ForMember(user => user.IsAccountEnabled, opt => opt.MapFrom(src => src.AccountEnabled))
                        .ForMember(user => user.CompanyInformation, opt => opt.MapFrom(src => Mapper.Map<IUser, CompanyInformation>(src)))
                        .ForMember(user => user.ContactInformation, opt => opt.MapFrom(src => Mapper.Map<IUser, ContactInformation>(src)))
                        .ForMember(user => user.Metadata, opt => opt.MapFrom(src => Mapper.Map<IUser, UserMetadata>(src)));
                }
            );

            Mapper.AssertConfigurationIsValid();
        }
    }
}