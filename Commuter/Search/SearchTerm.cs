using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Search
{
    public class SearchTerm : IMessageHandler
    {
        private readonly Guid _searchTermId;
        private readonly string _text;

        private static MessageDispatcher<SearchTerm> _dispatcher = new MessageDispatcher<SearchTerm>()
            .On("SearchResult", (s, m) => s.HandleSearchResult(m))
            .On("Aggregate", (s, m) => s.HandleAggregate(m));

        public SearchTerm(Guid searchTermId, string text)
        {
            _searchTermId = searchTermId;
            _text = text;
        }

        public IEnumerable<IMessageHandler> Children => Enumerable.Empty<IMessageHandler>();

        public Guid GetObjectId()
        {
            return _searchTermId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
        }

        public void HandleMessage(Message message)
        {
        }

        private void HandleSearchResult(Message searchResultMessage)
        {
            throw new NotImplementedException();
        }

        private void HandleAggregate(Message aggregateMessage)
        {
            throw new NotImplementedException();
        }
    }
}
