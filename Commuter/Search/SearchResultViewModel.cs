using System;

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

        public string Title
        {
            get { return _searchResult.Title; }
        }

        public string Author
        {
            get { return _searchResult.Author; }
        }

        public string Subtitle
        {
            get { return _searchResult.Subtitle; }
        }

        public Uri ImageUri
        {
            get { return _searchResult.ImageUri; }
        }
    }
}