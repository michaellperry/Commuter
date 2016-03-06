using System;

namespace Commuter.FeedJob.Entities
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }

        public Guid UserGuid { get; set; }
        public int StartAtEpisodeId { get; set; }

        public int PodcastId { get; set; }
        public virtual Podcast Podcast { get; set; }
    }
}