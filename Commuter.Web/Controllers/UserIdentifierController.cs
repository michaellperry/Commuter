using System.Web.Configuration;
using System.Web.Http;

namespace Commuter.Web.Controllers
{
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
