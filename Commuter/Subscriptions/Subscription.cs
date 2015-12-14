using System;

namespace Commuter.Subscriptions
{
    class Subscription
    {
        private readonly Uri _feedUrl;

        public Subscription(Uri feedUrl)
        {
            _feedUrl = feedUrl;
        }

        public Uri FeedUrl => _feedUrl;

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
