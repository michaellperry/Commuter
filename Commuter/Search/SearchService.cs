using Assisticant.Collections;
using Assisticant.Fields;
using Commuter.DigitalPodcast;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Commuter.Search
{
    class SearchService : RoverMob.Tasks.Process
    {
        private Observable<string> _searchTerm = new Observable<string>();
        private Observable<string> _searchResultTerm = new Observable<string>();
        private ObservableList<SearchResult> _searchResults = new ObservableList<SearchResult>();
        private Observable<SearchResult> _selectedSearchResult = new Observable<SearchResult>();

        public string SearchTerm
        {
            get { return _searchTerm.Value; }
            set { _searchTerm.Value = value; }
        }

        public string SearchResultTerm
        {
            get { return _searchResultTerm.Value; }
        }

        public ImmutableList<SearchResult> SearchResults
        {
            get { return _searchResults.ToImmutableList(); }
        }

        public SearchResult SelectedSearchResult
        {
            get { return _selectedSearchResult.Value; }
            set { _selectedSearchResult.Value = value; }
        }

        public void BeginSearch()
        {
            Perform(async delegate
            {
                string searchTerm = _searchTerm.Value;

                await Task.Delay(1500);
                SearchResult[] results = new SearchResult[]
                {
                    SearchResult.RandomSearchResult(),
                    SearchResult.RandomSearchResult(),
                    SearchResult.RandomSearchResult()
                };

                _searchResultTerm.Value = searchTerm;
                _searchResults.Clear();
                _searchResults.AddRange(results);
            });
        }

        public void ClearSearchResults()
        {
            _searchTerm.Value = null;
            _searchResultTerm.Value = null;
            _searchResults.Clear();
            _selectedSearchResult.Value = null;
        }
    }
}
