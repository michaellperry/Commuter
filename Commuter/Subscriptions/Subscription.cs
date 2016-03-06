using Assisticant.Collections;
using Commuter.Details;
using RoverMob.Messaging;
using System;
using System.Collections.Immutable;

namespace Commuter.Subscriptions
{
    public class Subscription
    {
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
    }
}
