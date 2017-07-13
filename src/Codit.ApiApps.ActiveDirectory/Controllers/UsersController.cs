using System.Threading.Tasks;
using System.Web.Http;
using Codit.ApiApps.ActiveDirectory.Repositories;

namespace Codit.ApiApps.ActiveDirectory.Controllers
{
    [RoutePrefix("api/v1")]
    public class UsersController : ApiController
    {
        private readonly UserRepository userRepository = new UserRepository();

        [Route("users")]
        public async Task<IHttpActionResult> GetUsers()
        {
            var users = await userRepository.Get();
            return Ok(users);
        }

        [Route("user")]
        public async Task<IHttpActionResult> GetUser(string objectId)
        {
            var user = await userRepository.Get(objectId);
            return Ok(user);
        }
    }
}
