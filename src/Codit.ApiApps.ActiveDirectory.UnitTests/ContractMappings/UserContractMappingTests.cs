

using System.Collections.Generic;
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
            Assert.AreEqual(JobTitle, contractUser.JobTitle);
            Assert.AreEqual(Department, contractUser.Department);
            Assert.AreEqual(EmailAddress, contractUser.EmailAddress);
            Assert.AreEqual(MobilePhoneNumber, contractUser.MobilePhoneNumber);
            Assert.AreEqual(OfficePhoneNumber, contractUser.OfficePhoneNumber);
            Assert.AreEqual(CompanyName, contractUser.CompanyName);
            Assert.AreEqual(Country, contractUser.Country);
            Assert.AreEqual(UserPrincipalName, contractUser.UserPrincipalName);
            Assert.AreEqual(ObjectId, contractUser.ObjectId);
            Assert.AreEqual(UserType, contractUser.UserType);
            Assert.AreEqual(ObjectType, contractUser.ObjectType);
            Assert.AreEqual(isAccountEnabled, contractUser.IsAccountEnabled);
        }
    }
}