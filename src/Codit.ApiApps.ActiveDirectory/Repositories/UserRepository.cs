using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Codit.ApiApps.Common;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.Data.OData;
using User = Codit.ApiApps.ActiveDirectory.Contracts.v1.User;

namespace Codit.ApiApps.ActiveDirectory.Repositories
{
    public class UserRepository : ActiveDirectoryRepository
    {
        /// <summary>
        ///     Gets a specific user by user pricinple name
        /// </summary>
        /// <param name="userPrincipleName">User principle name of the user</param>
        public async Task<Maybe<User>> Get(string userPrincipleName)
        {
            Guard.Guard.NotNullOrEmpty(userPrincipleName, nameof(userPrincipleName),
                "No user principle name was specified");

            var activeDirectoryClient = GetActiveDirectoryClient();

            try
            {
                var foundUsers = await activeDirectoryClient.Users
                    .Where(usr => usr.UserPrincipalName.Equals(userPrincipleName,
                        StringComparison.InvariantCultureIgnoreCase)).ExecuteAsync();

                if (foundUsers?.CurrentPage == null || foundUsers.CurrentPage.Any() == false)
                {
                    return new Maybe<User>();
                }

                if (foundUsers.CurrentPage.Count > 1)
                {
                    throw new InvalidOperationException(
                        $"More than one user was found with user principle name '{userPrincipleName}'");
                }

                var foundUser = foundUsers.CurrentPage.First();
                var user = Mapper.Map<IUser, User>(foundUser);

                return new Maybe<User>(user);
            }
            catch (ODataErrorException oDataErrorException)
            {
                var objectNotFoundMessage =
                    $"Resource '{userPrincipleName}' does not exist or one of its queried reference-property objects are not present.";

                if (oDataErrorException.Message.Contains(objectNotFoundMessage))
                {
                    return new Maybe<User>();
                }

                throw;
            }
        }

        /// <summary>
        ///     Gets all users
        /// </summary>
        public async Task<List<User>> GetAll(string companyName)
        {
            var activeDirectoryClient = GetActiveDirectoryClient();

            var usersPage = await activeDirectoryClient.Users.ExecuteAsync();
            var foundUsers = FilterMatchingUsers(usersPage, companyName);

            if (!usersPage.MorePagesAvailable)
            {
                return foundUsers;
            }

            do
            {
                usersPage = await usersPage.GetNextPageAsync();
                var filteredUsers = FilterMatchingUsers(usersPage, companyName);
                foundUsers.AddRange(filteredUsers);
            } while (usersPage.MorePagesAvailable);

            return foundUsers;
        }

        private List<User> FilterMatchingUsers(IPagedCollection<IUser> usersPage, string companyName)
        {
            var filteredUsers = FilterUsers(usersPage, companyName);
            var users = MapUsers(filteredUsers);
            return users;
        }

        private static IReadOnlyList<IUser> FilterUsers(IPagedCollection<IUser> usersPage, string companyName)
        {
            var filteredUsers = usersPage.CurrentPage;

            if (!string.IsNullOrWhiteSpace(companyName))
            {
                filteredUsers = filteredUsers.Where(user => !string.IsNullOrWhiteSpace(user.CompanyName) &&
                                                            user.CompanyName.Equals(companyName,
                                                                StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            }

            return filteredUsers;
        }

        private List<User> MapUsers(IReadOnlyList<IUser> pagedUsersResult)
        {
            var users = new List<User>();

            var mappedUsers = pagedUsersResult.Select(Mapper.Map<IUser, User>);
            users.AddRange(mappedUsers);

            return users;
        }
    }
}