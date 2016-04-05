using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.MyCommute
{
    class MyCommuteViewModel
    {
        private readonly Subscriptions.SubscriptionService _subscriptions;
        private readonly Search.SearchService _search;
        private readonly Images.ImageCacheService _imageCacheService;
        private readonly CommuterApplication _application;
        private readonly Media.MediaCacheService _mediaCacheService;

        public MyCommuteViewModel(
            Subscriptions.SubscriptionService subscriptions,
            Search.SearchService search,
            Images.ImageCacheService imageCacheService,
            CommuterApplication application,
            Media.MediaCacheService mediaCacheService)
        {
            _subscriptions = subscriptions;
            _search = search;
            _imageCacheService = imageCacheService;
            _application = application;
            _mediaCacheService = mediaCacheService;
        }

        private Queue CurrentQueue => _application.Root.QueuedEpisodes
            .Where(q => q.IsDownloaded)
            .FirstOrDefault();

        public Uri ImageUrl => _imageCacheService.GetCachedImageUri(
            CurrentQueue?.ImageUri);
        public string Title => CurrentQueue?.Title;
        public string Summary => CurrentQueue?.Summary;
        public string Media => CurrentQueue?.FileName;

        public void ManageSubscriptions()
        {
            _subscriptions.ManagingSubscriptions = true;
            _search.ClearSearchResults();
        }
    }
}
