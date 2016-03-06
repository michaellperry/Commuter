using Assisticant.Collections;
using Assisticant.Fields;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Commuter.Subscriptions
{
    class SubscriptionService
    {
        private readonly CommuterApplication _application;

        private Observable<bool> _managingSubscriptions = new Observable<bool>();
        private Observable<Subscription> _selectedSubscription = new Observable<Subscription>();

        public SubscriptionService(CommuterApplication application)
        {
            _application = application;
        }

        public ImmutableList<Subscription> Subscriptions =>
            _application.Root?.Subscriptions ??
            ImmutableList<Subscription>.Empty;

        public bool ManagingSubscriptions
        {
            get { return _managingSubscriptions.Value; }
            set { _managingSubscriptions.Value = value; }
        }

        public Subscription SelectedSubscription
        {
            get { return _selectedSubscription.Value; }
            set { _selectedSubscription.Value = value; }
        }

        public bool IsSubscribed(Uri feedUrl)
        {
            return Subscriptions.Any(s => s.FeedUrl == feedUrl);
        }

        public void Subscribe(Uri feedUrl, Uri imageUri, string title, string author)
        {
            if (_application.Root != null)
            {
                _application.EmitMessage(Message.CreateMessage(
                    _application.Root.GetObjectId().ToCanonicalString(),
                    "Subscribe",
                    _application.Root.GetObjectId(),
                    new
                    {
                        CreatedAt = DateTime.Now,
                        FeedUrl = feedUrl.ToString(),
                        ImageUri = imageUri.ToString(),
                        Title = title,
                        Author = author
                    }));
            }
        }

        public void Unsubscribe(Uri feedUrl)
        {
            if (_application.Root != null)
            {
                var subscription = _application.Root.Subscriptions
                    .FirstOrDefault(s => s.FeedUrl == feedUrl);
                _application.EmitMessage(Message.CreateMessage(
                    _application.Root.GetObjectId().ToCanonicalString(),
                    "Ubsubscribe",
                    Predecessors.Set
                        .In("Subscription", subscription.Hash),
                    _application.Root.GetObjectId(),
                    new { }));
            }
        }
    }
}