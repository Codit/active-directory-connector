using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
        [SwaggerResponse(HttpStatusCode.OK, "Returns all users", typeof(List<string>))]
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
        [Route("user")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns found user", typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound, "User with specified objectId was not found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "We were unable to successfully process the request")]
        public async Task<IHttpActionResult> GetUser(string objectId)
        {
            var user = await _userRepository.Get(objectId);
            return Ok(user);
        }
    }
}