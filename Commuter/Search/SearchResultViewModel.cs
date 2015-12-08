namespace Commuter.Search
{
    class SearchResultViewModel
    {
        private readonly SearchResult _searchResult;

        public SearchResultViewModel(SearchResult searchResult)
        {
            _searchResult = searchResult;
        }

        public SearchResult SearchResult
        {
            get { return _searchResult; }
        }
    }
}