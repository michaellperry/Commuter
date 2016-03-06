using System;
using RoverMob.Messaging;

namespace Commuter.Subscriptions
{
    public class Subscription
    {
        private readonly Uri _feedUrl;
        private readonly MessageHash _hash;

        public Subscription(Uri feedUrl, MessageHash hash)
        {
            _feedUrl = feedUrl;
            _hash = hash;
        }

        public Uri FeedUrl => _feedUrl;
        public MessageHash Hash => _hash;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (Subscription)obj;
            return this._feedUrl.Equals(that._feedUrl);
        }

        public override int GetHashCode()
        {
            return _feedUrl.GetHashCode();
        }
    }
}
