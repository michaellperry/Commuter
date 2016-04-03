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

        public MyCommuteViewModel(
            Subscriptions.SubscriptionService subscriptions,
            Search.SearchService search,
            Images.ImageCacheService imageCacheService)
        {
            _subscriptions = subscriptions;
            _search = search;
            _imageCacheService = imageCacheService;
        }

        private Queue CurrentQueue => null;

        public Uri ImageUrl => _imageCacheService.GetCachedImageUri(
            CurrentQueue?.ImageUri);

        public void ManageSubscriptions()
        {
            _subscriptions.ManagingSubscriptions = true;
            _search.ClearSearchResults();
        }
    }
}
