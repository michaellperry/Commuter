using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.MyCommute
{
    class MyCommuteViewModel
    {
        private Subscriptions.SubscriptionService _subscriptions;

        public MyCommuteViewModel(Subscriptions.SubscriptionService subscriptions)
        {
            _subscriptions = subscriptions;
        }

        public void ManageSubscriptions()
        {
            _subscriptions.ManagingSubscriptions = true;
        }
    }
}
