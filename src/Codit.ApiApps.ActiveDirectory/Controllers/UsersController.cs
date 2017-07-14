using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Codit.ApiApps.ActiveDirectory.Contracts.v1;
using Codit.ApiApps.ActiveDirectory.Repositories;
using Swashbuckle.Swagger.Annotations;

namespace Codit.ApiApps.ActiveDirectory.Controllers
{
    [RoutePrefix("api/v1")]
    public class UsersController : ApiController
    {
        private readonly UserRepository _userRepository = new UserRepository();

        /// <summary>
        ///     Gets all users in Active Directory
        /// </summary>
        [Route("users")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns all users", typeof(List<User>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No users were found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "We were unable to successfully process the request")]
        public async Task<IHttpActionResult> GetUsers()
        {
            var users = await _userRepository.Get();

            return users.Any() ? (IHttpActionResult)Ok(users) : NotFound();
        }

        /// <summary>
        ///     Gets a specific user
        /// </summary>
        /// <param name="objectId">Object Id for the user to lookup</param>
        [Route("users/{objectId}")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns found user", typeof(User))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Specified object id was not valid")]
        [SwaggerResponse(HttpStatusCode.NotFound, "User with specified objectId was not found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "We were unable to successfully process the request")]
        public async Task<IHttpActionResult> GetUserByObjectId(string objectId)
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return BadRequest("Object id was not specified");
            }

            var potenatialUser = await _userRepository.Get(objectId);
            return potenatialUser.IsPresent ? (IHttpActionResult)Ok(potenatialUser.Value) : NotFound();
        }

        /// <summary>
        ///     Gets a specific user
        /// </summary>
        /// <param name="firstName">First name of the user</param>
        /// <param name="lastName">Last name of the user</param>
        [Route("users/{lastName}/{firstName}")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns found user", typeof(User))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Specified first and/or last name were not valid")]
        [SwaggerResponse(HttpStatusCode.NotFound, "User was not found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "We were unable to successfully process the request")]
        public async Task<IHttpActionResult> GetUserByFirstAndLastName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return BadRequest("First name was not specified");
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return BadRequest("Last name was not specified");
            }

            var potenatialUser = await _userRepository.Get(firstName, lastName);
            return potenatialUser.IsPresent ? (IHttpActionResult)Ok(potenatialUser.Value) : NotFound();
        }
    }
}