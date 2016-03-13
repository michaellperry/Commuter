using Microsoft.AspNet.Identity;
using System.Web.Configuration;
using System.Web.Http;

namespace Commuter.Web.Controllers
{
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [Authorize]
    public class UserIdentifierController : RoverMob.Distributor.Controllers.UserIdentifierController
    {
        public UserIdentifierController() : base(
            WebConfigurationManager.AppSettings["StorageConnectionString"],
            "User")
        {
            
        }

        protected override bool AuthorizeUserForGet(string requestedUserId, string userId)
        {
            return true;
        }
    }
}
