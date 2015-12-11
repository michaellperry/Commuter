using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commuter.Subscriptions
{
    class SubscriptionDetailViewModel
    {
        private readonly Subscription _subscription;

        public SubscriptionDetailViewModel(Subscription subscription)
        {
            _subscription = subscription;
        }

        public Subscription Subscription
        {
            get { return _subscription; }
        }
    }
}
