using System;
using System.Collections.Immutable;
using System.Linq;

namespace Commuter.Subscriptions
{
    class SubscriptionViewModel
    {
        private readonly SubscriptionService _subscription;
        private readonly Search.SearchService _search;
        private readonly Func<Subscription, SubscriptionDetailViewModel> _newSubscriptionDetailViewModel;

        public SubscriptionViewModel(
            SubscriptionService subscription,
            Search.SearchService search,
            Func<Subscription, SubscriptionDetailViewModel> newSubscriptionDetailViewModel)
        {
            _subscription = subscription;
            _search = search;
            _newSubscriptionDetailViewModel = newSubscriptionDetailViewModel;
        }

        public string SearchTerm
        {
            get { return _search.SearchTerm; }
            set { _search.SearchTerm = value; }
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

        public bool HasSelectedSubscription =>
            _subscription.SelectedSubscription != null;

        public bool CanSubscribe =>
            _subscription.SelectedSubscription != null &&
            !_subscription.IsSubscribed(_subscription.SelectedSubscription.FeedUrl);

        public bool CanUnsubscribe =>
            _subscription.SelectedSubscription != null &&
            _subscription.IsSubscribed(_subscription.SelectedSubscription.FeedUrl);

        public void Subscribe()
        {
            if (_subscription.SelectedSubscription != null)
            {
                _subscription.Subscribe(_subscription.SelectedSubscription.FeedUrl);
            }
        }

        public void Unsubscribe()
        {
            if (_subscription.SelectedSubscription != null)
            {
                _subscription.Unsubscribe(_subscription.SelectedSubscription.FeedUrl);
            }
        }

        public void GoBack()
        {
            if (_subscription.SelectedSubscription != null)
                _subscription.SelectedSubscription = null;
            else
                _subscription.ManagingSubscriptions = false;
        }

        public void QuerySubmitted()
        {
            _search.BeginSearch();
        }
    }
}
