using Commuter.Search;
using Commuter.Subscriptions;
using System.Collections.Immutable;

namespace Commuter
{
    internal class Model
    {
        public ImmutableList<Subscription> Subscriptions { get; internal set; }
        public ImmutableList<SearchResult> SearchResults { get; internal set; }
        public bool ManagingSubscriptions { get; internal set; }
    }
}