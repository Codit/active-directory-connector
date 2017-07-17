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
        ///     Gets a specific user
        /// </summary>
        /// <param name="objectId">Object Id of the user</param>
        public async Task<Maybe<User>> Get(string objectId)
        {
            Guard.AgainstNullOrWhitespace(objectId, nameof(objectId));

            ActiveDirectoryClient activeDirectoryClient = GetActiveDirectoryClient();
            try
            {
                IUser foundUser = await activeDirectoryClient.Users.GetByObjectId(objectId).ExecuteAsync();

                if (foundUser == null)
                {
                    return new Maybe<User>();
                }

                User user = Mapper.Map<IUser, User>(foundUser);

                return new Maybe<User>(user);
            }
            catch (ODataErrorException oDataErrorException)
            {
                string objectNotFoundMessage =
                    $"Resource '{objectId}' does not exist or one of its queried reference-property objects are not present.";

                if (oDataErrorException.Message.Contains(objectNotFoundMessage))
                {
                    return new Maybe<User>();
                }

                throw;
            }
        }

        /// <summary>
        ///     Gets a specific user
        /// </summary>
        /// <param name="firstName">First name of the user</param>
        /// <param name="lastName">Last name of the user</param>
        public async Task<Maybe<User>> Get(string firstName, string lastName)
        {
            Guard.AgainstNullOrWhitespace(firstName, nameof(firstName));
            Guard.AgainstNullOrWhitespace(lastName, nameof(lastName));

            ActiveDirectoryClient activeDirectoryClient = GetActiveDirectoryClient();
            IPagedCollection<IUser> foundUsers = await activeDirectoryClient.Users
                .Where(usr => usr.GivenName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase)
                && usr.Surname.Equals(lastName, StringComparison.InvariantCultureIgnoreCase)).ExecuteAsync();

            if (foundUsers?.CurrentPage == null || foundUsers.CurrentPage.Any() == false)
            {
                return new Maybe<User>();
            }

            if (foundUsers.CurrentPage.Count > 1)
            {
                throw new InvalidOperationException($"More than one user was found with first name '{firstName}' and last name '{lastName}'");
            }

            IUser foundUser = foundUsers.CurrentPage.First();
            User user = Mapper.Map<IUser, User>(foundUser);

            return new Maybe<User>(user);
        }

        /// <summary>
        ///     Gets all users
        /// </summary>
        public async Task<List<User>> Get()
        {
            ActiveDirectoryClient activeDirectoryClient = GetActiveDirectoryClient();

            var foundUsers = new List<User>();
            IPagedCollection<IUser> usersPage = await activeDirectoryClient.Users.ExecuteAsync();

            while (usersPage.MorePagesAvailable)
            {
                IEnumerable<User> userNamesInCurrentPage = usersPage.CurrentPage.Select(Mapper.Map<IUser, User>);
                foundUsers.AddRange(userNamesInCurrentPage);

                usersPage = await usersPage.GetNextPageAsync();
            }

            return foundUsers;
        }
    }
}