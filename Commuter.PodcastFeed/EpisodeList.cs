using System;
using System.Collections.Immutable;

namespace Commuter.PodcastFeed
{
    public class EpisodeList
    {
        public Uri ImageUri { get; set; }
        public ImmutableList<Episode> Episodes { get; set; }
    }
}