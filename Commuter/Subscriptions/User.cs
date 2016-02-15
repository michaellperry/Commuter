using Assisticant.Collections;
using Commuter.Search;
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

        private ObservableList<SearchTerm> _searchTerms = new ObservableList<SearchTerm>();

        private static MessageDispatcher<User> _dispatcher = new MessageDispatcher<User>()
            .On("SearchTerm", (u, m) => u.HandleSearchTerm(m));

        public User(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<SearchTerm> SearchTerms => _searchTerms;

        public IEnumerable<IMessageHandler> Children => _searchTerms;

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

        private void HandleSearchTerm(Message searchTermMessage)
        {
            string text = searchTermMessage.Body.Text;

            if (!_searchTerms.Any(t => t.GetObjectId() == searchTermMessage.ObjectId))
                _searchTerms.Add(new SearchTerm(searchTermMessage.ObjectId, text));
        }
    }
}
