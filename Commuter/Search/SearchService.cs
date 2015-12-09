using Assisticant.Collections;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Search
{
    class SearchService : RoverMob.Tasks.Process
    {
        private Observable<string> _searchTerm = new Observable<string>();
        private ObservableList<SearchResult> _searchResults = new ObservableList<SearchResult>();
        private Observable<SearchResult> _selectedSearchResult = new Observable<SearchResult>();

        public string SearchTerm
        {
            get { return _searchTerm.Value; }
            set { _searchTerm.Value = value; }
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
                await Task.Delay(1500);
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
            _selectedSearchResult.Value = null;
        }
    }
}
