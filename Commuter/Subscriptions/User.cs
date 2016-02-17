using Assisticant.Collections;
using Assisticant.Fields;
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

        private Observable<SearchTerm> _searchTerm = new Observable<SearchTerm>();

        public User(Guid userId)
        {
            _userId = userId;
        }

        public SearchTerm SearchTerm
        {
            get { return _searchTerm.Value; }
            set { _searchTerm.Value = value; }
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
        }
    }
}
