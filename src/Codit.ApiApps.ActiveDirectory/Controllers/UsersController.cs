using System.Web.Http;

namespace Codit.ApiApps.ActiveDirectory.Controllers
{
    [RoutePrefix("api/v1")]
    public class UsersController : ApiController
    {
        [Route("users")]
        public IHttpActionResult Get()
        {
            return NotFound();
        }
    }
}
