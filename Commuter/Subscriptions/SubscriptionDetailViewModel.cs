using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commuter.Images;
using Commuter.Details;
using System.Collections.Immutable;

namespace Commuter.Subscriptions
{
    class SubscriptionDetailViewModel
    {
        private readonly Subscription _subscription;
        private readonly ImageCacheService _imageCacheService;
        private readonly Func<Episode, EpisodeViewModel> _newEpisodeViewModel;

        public SubscriptionDetailViewModel(
            Subscription subscription,
            ImageCacheService imageCacheService,
            Func<Episode, EpisodeViewModel> newEpisodeViewModel)
        {
            _subscription = subscription;
            _imageCacheService = imageCacheService;
            _newEpisodeViewModel = newEpisodeViewModel;
        }

        public Subscription Subscription
        {
            get { return _subscription; }
        }

        public Uri ImageUri => _imageCacheService.GetCachedImageUri(
            _subscription.ImageUri);
        public string Title => _subscription.Title;
        public string Subtitle => _subscription.Subtitle;
        public string Author => _subscription.Author;

        public ImmutableList<EpisodeViewModel> Episodes =>
            (from episode in _subscription.Episodes
             orderby episode.PublishDate descending
             select _newEpisodeViewModel(episode))
            .ToImmutableList();

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
