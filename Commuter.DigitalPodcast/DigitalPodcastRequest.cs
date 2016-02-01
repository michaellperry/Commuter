using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.DigitalPodcast
{
    public class DigitalPodcastRequest
    {
        public string Keywords { get; set; }
        public int Start { get; set; } = 0;
        public int Results { get; set; } = 10;
        public SortOrder Sort { get; set; } = SortOrder.Relavance;
        public SearchSource Source { get; set; } = SearchSource.All;
        public ContentFilter Filter { get; set; } = ContentFilter.NoFilter;
    }
}
