using AutoMapper;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using NUnit.Framework;

namespace Codit.ApiApps.ActiveDirectory.UnitTests.ContractMappings
{
    public class UserContractMappingTests : ContractMappingTest
    {
        [Test]
        public void MapActiveDirectoryUserToContractUser_ValidInput_MappingSucceeds()
        {
            // Arrange
            const string FirstName = "John";
            const string LastName = "Doe";
            string displayName = $"{FirstName} {LastName}";
            const string JobTitle = "Recruiter";
            const string Department = "HR";
            const string CompanyName = "Codit";
            const string EmailAddress = "John.Doe@codit.eu";
            const string OfficePhoneNumber = "+32 475 123456";
            const string MobilePhoneNumber = "+32 475 123456";
            const string Country = "Belgium";
            const string UserPrincipalName = "John.Doe@codit.eu";
            const string ObjectId = "f060a470-21ea-4e2b-95b5-d0fbc2cc8853";
            const string UserType = "Member";
            const string ObjectType = "User";
            bool? isAccountEnabled = true;

            User activeDirectoryUser = new User
            {
                GivenName = FirstName,
                Surname = LastName,
                CompanyName = CompanyName,
                JobTitle = JobTitle,
                Department = Department,
                Mail = EmailAddress,
                Country = Country,
                DisplayName = displayName,
                UserPrincipalName = UserPrincipalName,
                ObjectId = ObjectId,
                UserType = UserType,
                ObjectType = ObjectType,
                TelephoneNumber = OfficePhoneNumber,
                Mobile = MobilePhoneNumber,
                AccountEnabled = isAccountEnabled
            };

            // Act
            Contracts.v1.User contractUser = Mapper.Map<IUser, Contracts.v1.User>(activeDirectoryUser);

            // Assert
            Assert.NotNull(contractUser);
            Assert.AreEqual(FirstName, contractUser.FirstName);
            Assert.AreEqual(LastName, contractUser.LastName);
            Assert.AreEqual(displayName, contractUser.DisplayName);
            Assert.AreEqual(isAccountEnabled, contractUser.IsAccountEnabled);
            Assert.AreEqual(UserPrincipalName, contractUser.UserPrincipalName);
            Assert.NotNull(contractUser.CompanyInformation);
            Assert.AreEqual(JobTitle, contractUser.CompanyInformation.JobTitle);
            Assert.AreEqual(Department, contractUser.CompanyInformation.Department);
            Assert.AreEqual(CompanyName, contractUser.CompanyInformation.CompanyName);
            Assert.AreEqual(Country, contractUser.CompanyInformation.Country);
            Assert.NotNull(contractUser.ContactInformation);
            Assert.AreEqual(EmailAddress, contractUser.ContactInformation.EmailAddress);
            Assert.AreEqual(MobilePhoneNumber, contractUser.ContactInformation.MobilePhoneNumber);
            Assert.AreEqual(OfficePhoneNumber, contractUser.ContactInformation.OfficePhoneNumber);
            Assert.NotNull(contractUser.Metadata);
            Assert.AreEqual(UserType, contractUser.Metadata.UserType);
            Assert.AreEqual(ObjectId, contractUser.Metadata.ObjectId);
            Assert.AreEqual(ObjectType, contractUser.Metadata.ObjectType);
        }
    }
}