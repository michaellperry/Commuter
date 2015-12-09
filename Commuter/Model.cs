using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using System.Collections.Immutable;
using System;
using System.Threading.Tasks;

namespace Commuter
{
    internal class Model
    {
        private readonly Search.SearchService _searchService = new Search.SearchService();
        private readonly Subscriptions.SubscriptionService _subscriptionService = new Subscriptions.SubscriptionService();

        public Search.SearchService SearchService
        {
            get { return _searchService; }
        }

        public Subscriptions.SubscriptionService SubscriptionService
        {
            get { return _subscriptionService; }
        }
    }
}