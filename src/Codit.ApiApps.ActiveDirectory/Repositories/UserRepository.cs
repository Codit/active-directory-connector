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
            Guard.AgainstNullOrWhitespace(userPrincipleName, nameof(userPrincipleName));

            ActiveDirectoryClient activeDirectoryClient = GetActiveDirectoryClient();
            try
            {
                IPagedCollection<IUser> foundUsers = await activeDirectoryClient.Users.Where(usr => usr.UserPrincipalName.Equals(userPrincipleName, StringComparison.InvariantCultureIgnoreCase)).ExecuteAsync();

                if (foundUsers?.CurrentPage == null || foundUsers.CurrentPage.Any() == false)
                {
                    return new Maybe<User>();
                }

                if (foundUsers.CurrentPage.Count > 1)
                {
                    throw new InvalidOperationException($"More than one user was found with user principle name '{userPrincipleName}'");
                }

                IUser foundUser = foundUsers.CurrentPage.First();
                User user = Mapper.Map<IUser, User>(foundUser);

                return new Maybe<User>(user);
            }
            catch (ODataErrorException oDataErrorException)
            {
                string objectNotFoundMessage =
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
        public async Task<List<User>> Get()
        {
            ActiveDirectoryClient activeDirectoryClient = GetActiveDirectoryClient();

            var foundUsers = new List<User>();

            IPagedCollection<IUser> usersPage = await activeDirectoryClient.Users.ExecuteAsync();
            AddPagedUsersToFoundUsers(foundUsers, usersPage.CurrentPage);

            do
            {
                usersPage = await usersPage.GetNextPageAsync();
                AddPagedUsersToFoundUsers(foundUsers, usersPage.CurrentPage);
            } while (usersPage.MorePagesAvailable);

            return foundUsers;
        }

        private void AddPagedUsersToFoundUsers(List<User> foundUsers, IReadOnlyList<IUser> pagedUsersResult)
        {
            var mappedUsers = pagedUsersResult.Select(Mapper.Map<IUser, User>);
            foundUsers.AddRange(mappedUsers);
        }
    }
}