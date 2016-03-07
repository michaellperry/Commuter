using System;
using System.Collections.Generic;

namespace Commuter.FeedJob.Entities
{
    public class Podcast
    {
        public int PodcastId { get; set; }

        public string FeedUrl { get; set; }
        public DateTime? LastUpdateDateTime { get; set; }
        public DateTime? LastAttemptDateTime { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }
    }
}