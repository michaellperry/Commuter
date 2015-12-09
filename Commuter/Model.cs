using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Commuter.Subscriptions;
using System.Collections.Immutable;
using System;
using System.Threading.Tasks;

namespace Commuter
{
    internal class Model
    {
        private ObservableList<Subscription> _subscriptions = new ObservableList<Subscription>();
        private Observable<bool> _managingSubscriptions = new Observable<bool>();

        private Search.SearchService _searchService = new Search.SearchService();

        public ImmutableList<Subscription> Subscriptions
        {
            get { return _subscriptions.ToImmutableList(); }
        }

        public bool ManagingSubscriptions
        {
            get { return _managingSubscriptions.Value; }
        }

        public Search.SearchService SearchService
        {
            get { return _searchService; }
        }
    }
}