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

        public SearchResult SearchResult => _searchResult;
        public string Title => _searchResult.Title;
        public string Author => _searchResult.Author;
        public string Subtitle => _searchResult.Subtitle;
        public Uri ImageUri => _imageCacheService.GetCachedImageUri(_searchResult.ImageUri);
    }
}