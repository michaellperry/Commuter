using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commuter.Images;

namespace Commuter.Subscriptions
{
    class SubscriptionDetailViewModel
    {
        private readonly Subscription _subscription;
        private readonly ImageCacheService _imageCacheService;

        public SubscriptionDetailViewModel(
            Subscription subscription,
            ImageCacheService imageCacheService)
        {
            _subscription = subscription;
            _imageCacheService = imageCacheService;
        }

        public Subscription Subscription
        {
            get { return _subscription; }
        }

        public Uri ImageUri => _imageCacheService.GetCachedImageUri(
            _subscription.ImageUri);
        public string Title => _subscription.Title;
        public string Author => _subscription.Author;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (SubscriptionDetailViewModel)obj;
            return this._subscription.Equals(that._subscription);
        }

        public override int GetHashCode()
        {
            return _subscription.GetHashCode();
        }
    }
}
