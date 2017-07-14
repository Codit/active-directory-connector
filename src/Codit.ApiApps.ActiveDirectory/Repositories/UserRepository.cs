using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<User> Get(string objectId)
        {
            var activeDirectoryClient = GetActiveDirectoryClient();
            var foundUser = await activeDirectoryClient.Users.GetByObjectId(objectId).ExecuteAsync();
            var user = MapUserToExternalContract(foundUser);

            return user;
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
                Type = activeDirectoryUser.UserType
            };

            return user;
        }
    }
}