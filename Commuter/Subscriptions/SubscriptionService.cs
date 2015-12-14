﻿using Assisticant.Collections;
using Assisticant.Fields;
using System;
using System.Collections.Immutable;

namespace Commuter.Subscriptions
{
    class SubscriptionService
    {
        private ObservableList<Subscription> _subscriptions = new ObservableList<Subscription>();
        private Observable<bool> _managingSubscriptions = new Observable<bool>();
        private Observable<Subscription> _selectedSubscription = new Observable<Subscription>();

        public ImmutableList<Subscription> Subscriptions
        {
            get { return _subscriptions.ToImmutableList(); }
        }

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
            return _subscriptions.Contains(new Subscription(feedUrl));
        }

        public void Subscribe(Uri feedUrl)
        {
            _subscriptions.Add(new Subscription(feedUrl));
        }

        public void Unsubscribe(Uri feedUrl)
        {
            _subscriptions.Remove(new Subscription(feedUrl));
        }
    }
}