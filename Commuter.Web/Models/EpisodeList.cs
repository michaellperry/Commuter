using System;
using System.Collections.Immutable;

namespace Commuter.Web.Models
{
    public class EpisodeList
    {
        public ImmutableList<Episode> Episodes { get; set; }
    }
}