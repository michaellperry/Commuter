using System.Threading.Tasks;
using System.Web.Configuration;

namespace Commuter.Web.Controllers
{
    public class DistributorController : RoverMob.Distributor.Controllers.DistributorController
    {
        protected DistributorController() : base(
            WebConfigurationManager.AppSettings["StorageConnectionString"],
            WebConfigurationManager.AppSettings["NotificationConnectionString"],
            WebConfigurationManager.AppSettings["ServiceBusConnectionString"],
            WebConfigurationManager.AppSettings["ServiceBusPath"])
        {

        }

        protected override Task<bool> AuthorizeUserForGet(string topic, string userId)
        {
            return Task.FromResult(true);
        }

        protected override Task<bool> AuthorizeUserForPost(string topic, string userId)
        {
            return Task.FromResult(true);
        }
    }
}
