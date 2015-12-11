using System;
using System.Collections.Immutable;
using System.Linq;

namespace Commuter.Subscriptions
{
    class SubscriptionViewModel
    {
        private readonly SubscriptionService _subscription;
        private readonly Func<Subscription, SubscriptionDetailViewModel> _newSubscriptionDetailViewModel;

        public SubscriptionViewModel(
            SubscriptionService subscription,
            Func<Subscription, SubscriptionDetailViewModel> newSubscriptionDetailViewModel)
        {
            _subscription = subscription;
            _newSubscriptionDetailViewModel = newSubscriptionDetailViewModel;
        }

        public ImmutableList<SubscriptionDetailViewModel> Subscriptions
        {
            get
            {
                return (
                    from subscription in _subscription.Subscriptions
                    select _newSubscriptionDetailViewModel(subscription)
                    ).ToImmutableList();
            }
        }

        public SubscriptionDetailViewModel SelectedSubscription
        {
            get
            {
                return _subscription.SelectedSubscription == null
                    ? null
                    : _newSubscriptionDetailViewModel(_subscription.SelectedSubscription);
            }
            set
            {
                _subscription.SelectedSubscription = value == null
                    ? null
                    : value.Subscription;
            }
        }

        public bool HasSelectedSubscription
        {
            get { return _subscription.SelectedSubscription != null; }
        }

        public void GoBack()
        {
            if (_subscription.SelectedSubscription != null)
                _subscription.SelectedSubscription = null;
            else
                _subscription.ManagingSubscriptions = false;
        }
    }
}
