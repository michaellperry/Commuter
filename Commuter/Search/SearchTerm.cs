using Assisticant.Collections;
using Assisticant.Fields;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Commuter.Search
{
    public class SearchTerm : IMessageHandler
    {
        private readonly Guid _searchTermId;
        private readonly string _text;

        private Observable<Aggregate> _lastAggregate = new Observable<Aggregate>();
        private ObservableList<SearchResult> _searchResults = new ObservableList<SearchResult>();

        private static MessageDispatcher<SearchTerm> _dispatcher = new MessageDispatcher<SearchTerm>()
            .On("SearchResult", (s, m) => s.HandleSearchResult(m))
            .On("Aggregate", (s, m) => s.HandleAggregate(m));

        public SearchTerm(Guid searchTermId, string text)
        {
            _searchTermId = searchTermId;
            _text = text;
        }

        public string Text => _text;

        public ImmutableList<SearchResult> SearchResults => _searchResults
            .Where(r => _lastAggregate.Value != null && _lastAggregate.Value.IncludesSearchResult(r.Hash))
            .ToImmutableList();

        public bool IsBusy => _lastAggregate.Value == null;

        public IEnumerable<IMessageHandler> Children => Enumerable.Empty<IMessageHandler>();

        public Guid GetObjectId()
        {
            return _searchTermId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            foreach (var m in messages)
                HandleMessage(m);
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        private void HandleSearchResult(Message message)
        {
            _searchResults.Add(SearchResult.FromMessage(message));
        }

        private void HandleAggregate(Message message)
        {
            _lastAggregate.Value = Aggregate.FromMessage(message);
        }
    }
}
