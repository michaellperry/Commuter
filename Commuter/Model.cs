using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Commuter.Search;
using Commuter.Subscriptions;
using System.Collections.Immutable;

namespace Commuter
{
    internal class Model
    {
        private ObservableList<Subscription> _subscriptions = new ObservableList<Subscription>();
        private ObservableList<SearchResult> _searchResults = new ObservableList<SearchResult>();
        private Observable<bool> _managingSubscriptions = new Observable<bool>();

        public ImmutableList<Subscription> Subscriptions { get { return _subscriptions.ToImmutableList(); } }
        public ImmutableList<SearchResult> SearchResults { get { return _searchResults.ToImmutableList(); } }
        public bool ManagingSubscriptions { get { return _managingSubscriptions.Value; } }
    }
}