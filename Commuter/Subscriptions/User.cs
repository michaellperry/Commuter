using Assisticant.Collections;
using Assisticant.Fields;
using Commuter.Search;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commuter.Subscriptions
{
    public class User : IMessageHandler
    {
        private readonly Guid _userId;

        private Observable<SearchTerm> _searchTerm = new Observable<SearchTerm>();
        private Observable<SearchResult> _selectedSearchResult = new Observable<SearchResult>();

        private static MessageDispatcher<User> _dispatcher = new MessageDispatcher<User>()
            .On("Search", (u, m) => u.HandleSearch(m));

        public User(Guid userId)
        {
            _userId = userId;
        }

        public SearchTerm SearchTerm => _searchTerm.Value;

        public SearchResult SelectedSearchResult
        {
            get { return _selectedSearchResult.Value; }
            set { _selectedSearchResult.Value = value; }
        }

        public void ClearSearch()
        {
            _searchTerm.Value = null;
        }

        public IEnumerable<IMessageHandler> Children =>
            new IMessageHandler[] { SearchTerm }
                .Where(s => s != null);

        public Guid GetObjectId()
        {
            return _userId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        private void HandleSearch(Message message)
        {
        }
    }
}
