using System;
using RoverMob.Messaging;

namespace Commuter.Subscriptions
{
    public class Subscription
    {
        public Subscription(
            Uri feedUrl,
            Uri imageUri,
            string title,
            string author,
            MessageHash hash)
        {
            FeedUrl = feedUrl;
            ImageUri = imageUri;
            Title = title;
            Author = author;
            Hash = hash;
        }

        public Uri FeedUrl { get; }
        public Uri ImageUri { get; }
        public string Title { get; }
        public string Author { get; }
        public MessageHash Hash { get; }

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
