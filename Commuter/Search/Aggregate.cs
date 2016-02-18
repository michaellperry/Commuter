using RoverMob.Messaging;
using System.Collections.Immutable;

namespace Commuter.Search
{
    public class Aggregate
    {
        private readonly ImmutableHashSet<MessageHash> _hashes;

        private Aggregate(ImmutableHashSet<MessageHash> hashes)
        {
            _hashes = hashes;
        }

        public bool IncludesSearchResult(MessageHash hash)
        {
            return _hashes.Contains(hash);
        }

        public static Aggregate FromMessage(Message message)
        {
            var hashes = message.GetPredecessors("SearchResult");
            return new Aggregate(hashes.ToImmutableHashSet());
        }
    }
}
