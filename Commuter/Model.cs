using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using System.Collections.Immutable;
using System;
using System.Threading.Tasks;

namespace Commuter
{
    class Model
    {
        private readonly Search.SearchService _searchService;
        private readonly Subscriptions.SubscriptionService _subscriptionService;
        private readonly CommuterApplication _application;

        public Model(
            Search.SearchService searchService,
            Subscriptions.SubscriptionService subscriptionService,
            CommuterApplication application)
        {
            _searchService = searchService;
            _subscriptionService = subscriptionService;
            _application = application;
        }

        public Search.SearchService SearchService
        {
            get { return _searchService; }
        }

        public Subscriptions.SubscriptionService SubscriptionService
        {
            get { return _subscriptionService; }
        }

        public CommuterApplication Application
        {
            get { return _application; }
        }
    }
}