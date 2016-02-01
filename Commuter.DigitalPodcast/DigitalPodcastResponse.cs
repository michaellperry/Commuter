using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Commuter.DigitalPodcast
{
    public class DigitalPodcastResponse
    {
        public ImmutableList<DigitalPodcastResult> Results { get; set; }
    }
}
