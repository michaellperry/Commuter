using Assisticant.Collections;
using Commuter.Details;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Commuter.Subscriptions
{
    public class Subscription : IMessageHandler
    {
        private static MessageDispatcher<Subscription> _dispatcher =
            new MessageDispatcher<Subscription>()
            .On("Episode", (s, m) => s.HandleEpisode(m));

        private ObservableList<Episode> _episodes = new ObservableList<Episode>();

        public Subscription(
            Uri feedUrl,
            Uri imageUri,
            string title,
            string subtitle,
            string author,
            MessageHash hash)
        {
            FeedUrl = feedUrl;
            ImageUri = imageUri;
            Title = title;
            Subtitle = subtitle;
            Author = author;
            Hash = hash;
        }

        public Uri FeedUrl { get; }
        public Uri ImageUri { get; }
        public string Title { get; }
        public string Subtitle { get; }
        public string Author { get; }
        public MessageHash Hash { get; }

        public ImmutableList<Episode> Episodes =>
            _episodes.ToImmutableList();

        public IEnumerable<IMessageHandler> Children =>
            ImmutableList<IMessageHandler>.Empty;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (Subscription)obj;
            return this.FeedUrl.Equals(that.FeedUrl);
        }

        public override int GetHashCode()
        {
            return FeedUrl.GetHashCode();
        }

        public Guid GetObjectId()
        {
            return new { FeedUrl = FeedUrl }.ToGuid();
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            foreach (var message in messages)
                _dispatcher.Dispatch(this, message);
        }

        public void HandleEpisode(Message message)
        {
            if (!_episodes.Any(e => e.Hash == message.Hash))
                _episodes.Add(new Episode
                {
                    Title = message.Body.Title,
                    Summary = message.Body.Summary,
                    PublishDate = message.Body.PublishDate,
                    MediaUrl = new Uri(message.Body.MediaUrl, UriKind.Absolute),
                    Hash = message.Hash
                });
        }
    }
}
