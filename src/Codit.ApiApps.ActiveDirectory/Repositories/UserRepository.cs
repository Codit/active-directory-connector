using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codit.ApiApps.Common;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using User = Codit.ApiApps.ActiveDirectory.Contracts.v1.User;

namespace Codit.ApiApps.ActiveDirectory.Repositories
{
    public class UserRepository: ActiveDirectoryRepository
    {
        /// <summary>
        ///     Gets a specific user
        /// </summary>
        /// <param name="objectId">Object Id of the user</param>
        public async Task<Maybe<User>> Get(string objectId)
        {
            var activeDirectoryClient = GetActiveDirectoryClient();
            var foundUser = await activeDirectoryClient.Users.GetByObjectId(objectId).ExecuteAsync();

            if (foundUser == null)
            {
                return new Maybe<User>();
            }

            var user = MapUserToExternalContract(foundUser);

            return new Maybe<User>(user);
        }

        /// <summary>
        ///     Gets all users
        /// </summary>
        public async Task<List<User>> Get()
        {
            var activeDirectoryClient = GetActiveDirectoryClient();

            var foundUsers = new List<User>();
            var usersPage = await activeDirectoryClient.Users.ExecuteAsync();

            while (usersPage.MorePagesAvailable)
            {
                var userNamesInCurrentPage = usersPage.CurrentPage.Select(MapUserToExternalContract);
                foundUsers.AddRange(userNamesInCurrentPage);

                usersPage = await usersPage.GetNextPageAsync();
            }

            return foundUsers;
        }

        private static User MapUserToExternalContract(IUser activeDirectoryUser)
        {
            var user = new User
            {
                FirstName = activeDirectoryUser.GivenName,
                LastName = activeDirectoryUser.Surname,
                DisplayName = activeDirectoryUser.DisplayName,
                UserPrincipalName = activeDirectoryUser.UserPrincipalName,
                CompanyName = activeDirectoryUser.CompanyName,
                Country = activeDirectoryUser.Country,
                Type = activeDirectoryUser.UserType,
                ObjectId = activeDirectoryUser.ObjectId
            };

            return user;
        }
    }
}