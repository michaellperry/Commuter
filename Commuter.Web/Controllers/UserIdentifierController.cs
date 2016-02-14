using System.Web.Configuration;

namespace Commuter.Web.Controllers
{
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
