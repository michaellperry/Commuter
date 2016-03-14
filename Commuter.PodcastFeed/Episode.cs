using System;

namespace Commuter.PodcastFeed
{
    public class Episode
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime PublishDate { get; set; }
        public string MediaUrl { get; set; }
    }
}