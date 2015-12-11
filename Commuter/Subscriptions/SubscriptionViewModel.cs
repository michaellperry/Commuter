using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commuter.Subscriptions
{
    class SubscriptionViewModel
    {
        private readonly SubscriptionService _subscription;

        public SubscriptionViewModel(SubscriptionService subscription)
        {
            _subscription = subscription;
        }

        public void GoBack()
        {
            _subscription.ManagingSubscriptions = false;
        }
    }
}
