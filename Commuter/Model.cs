using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Commuter.Search;
using Commuter.Subscriptions;
using System.Collections.Immutable;
using System;
using System.Threading.Tasks;

namespace Commuter
{
    internal class Model : RoverMob.Tasks.Process
    {
        private ObservableList<Subscription> _subscriptions = new ObservableList<Subscription>();

        private Observable<string> _searchTerm = new Observable<string>();
        private ObservableList<SearchResult> _searchResults = new ObservableList<SearchResult>();
        private Observable<bool> _managingSubscriptions = new Observable<bool>();

        public ImmutableList<Subscription> Subscriptions
        {
            get { return _subscriptions.ToImmutableList(); }
        }

        public string SearchTerm
        {
            get { return _searchTerm.Value; }
            set { _searchTerm.Value = value; }
        }

        public ImmutableList<SearchResult> SearchResults
        {
            get { return _searchResults.ToImmutableList(); }
        }

        public bool ManagingSubscriptions
        {
            get { return _managingSubscriptions.Value; }
        }

        public void BeginSearch()
        {
            Perform(async delegate
            {
                await Task.Delay(500);
                _searchResults.Clear();
                _searchResults.AddRange(new SearchResult[]
                {
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult()
                });
            });
        }

        public void ClearSearchResults()
        {
            _searchResults.Clear();
        }
    }
}