using System.Threading.Tasks;
using System.Web.Configuration;
using RoverMob.Distributor.Dispatchers;
using RoverMob.Distributor.Filters;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Commuter.Web.Controllers
{
    public class DistributorController : RoverMob.Distributor.Controllers.DistributorController
    {
        protected DistributorController() : base(
            WebConfigurationManager.AppSettings["StorageConnectionString"])
        {
            AddDispatcher(new AzureNotificationHubDispatcher(
                WebConfigurationManager.AppSettings["NotificationConnectionString"]));
            AddDispatcher(
                new MessagesOfType("Search"),
                new AzureServiceBusQueueDispatcher(
                    WebConfigurationManager.AppSettings["ServiceBusConnectionString"],
                    "searchmessages"));
            AddDispatcher(
                new MessagesOfType("Subscribe", "Unsubscribe"),
                new AzureServiceBusQueueDispatcher(
                    WebConfigurationManager.AppSettings["ServiceBusConnectionString"],
                    "subscriptionmessages"));
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
