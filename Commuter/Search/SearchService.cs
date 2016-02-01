using Assisticant.Collections;
using Assisticant.Fields;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Commuter.DigitalPodcast;
using System.Linq;

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
                var searchTerm = _searchTerm.Value;
                var search = new DigitalPodcastSearch(
                    new Secrets().DigitalPodcastApiKey);
                var response = await search.SearchAsync(new DigitalPodcastRequest
                {
                    Keywords = searchTerm
                });
                _searchResultTerm.Value = searchTerm;
                _searchResults.Clear();
                _searchResults.AddRange(
                    response.Results.Select(r =>
                        new SearchResult(r.Title, r.FeedUrl)));
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
