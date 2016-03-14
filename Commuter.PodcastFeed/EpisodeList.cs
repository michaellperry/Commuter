using System;
using System.Collections.Immutable;

namespace Commuter.PodcastFeed
{
    public class EpisodeList
    {
        public ImmutableList<Episode> Episodes { get; set; }
    }
}