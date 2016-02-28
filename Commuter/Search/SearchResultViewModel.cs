using System;

namespace Commuter.Search
{
    class SearchResultViewModel
    {
        private readonly SearchResult _searchResult;
        private readonly Images.ImageCacheService _imageCacheService;

        public SearchResultViewModel(
            SearchResult searchResult,
            Images.ImageCacheService imageCacheService)
        {
            _searchResult = searchResult;
            _imageCacheService = imageCacheService;
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
            get { return _imageCacheService.GetCachedImageUri(_searchResult.ImageUri); }
        }
    }
}