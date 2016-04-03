using System;

namespace Commuter.FeedJob.Entities
{
    public class Episode
    {
        public int EpisodeId { get; set; }

        public int PodcastId { get; set; }
        public virtual Podcast Podcast { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime PublishDate { get; set; }
        public string MediaUrl { get; set; }
        public string ImageUri { get; set; }
    }
}