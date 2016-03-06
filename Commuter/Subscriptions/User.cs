using Assisticant.Collections;
using Assisticant.Fields;
using Commuter.Search;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Commuter.Subscriptions
{
    public class User : IMessageHandler
    {
        private readonly Guid _userId;

        private Observable<SearchTerm> _searchTerm = new Observable<SearchTerm>();
        private Observable<SearchResult> _selectedSearchResult = new Observable<SearchResult>();
        private SuccessorCollection<Subscription> _subscriptions;

        private static MessageDispatcher<User> _dispatcher = new MessageDispatcher<User>()
            .On("Search", (u, m) => u.HandleSearch(m));

        public User(Guid userId)
        {
            _userId = userId;
            _subscriptions = new SuccessorCollection<Subscription>(
                "Subscribe",
                CreateSubscription,
                "Unsubscribe",
                "Subscription");
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

        public ImmutableList<Subscription> Subscriptions =>
            _subscriptions.Items.ToImmutableList();

        public IEnumerable<IMessageHandler> Children =>
            new IMessageHandler[] { SearchTerm }
                .Where(s => s != null);

        public Guid GetObjectId()
        {
            return _userId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            _subscriptions.HandleAllMessages(messages);
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
            _subscriptions.HandleMessage(message);
        }

        private void HandleSearch(Message message)
        {
            string searchTerm = message.Body.SearchTerm;
            var searchTermId = new { Text = searchTerm }.ToGuid();
            _searchTerm.Value = new SearchTerm(
                searchTermId, searchTerm);
        }

        public Subscription CreateSubscription(Message message)
        {
            string feedUrl = message.Body.FeedUrl;
            string imageUri = message.Body.ImageUri;
            string title = message.Body.Title;
            string subtitle = message.Body.Subtitle;
            string author = message.Body.Author;
            return new Subscription(
                new Uri(feedUrl, UriKind.Absolute),
                new Uri(imageUri, UriKind.Absolute),
                title,
                subtitle,
                author,
                message.Hash);
        }
    }
}
