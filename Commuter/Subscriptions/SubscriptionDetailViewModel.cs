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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (SubscriptionDetailViewModel)obj;
            return this._subscription.Equals(that._subscription);
        }

        public override int GetHashCode()
        {
            return _subscription.GetHashCode();
        }
    }
}
