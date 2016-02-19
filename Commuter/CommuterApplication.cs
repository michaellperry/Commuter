using Assisticant.Collections;
using Commuter.Subscriptions;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Linq;
using System.Threading;

namespace Commuter
{
    public class CommuterApplication : Application<User>
    {
        private Timer _refreshTimer;

        private CommuterApplication()
        {

        }

        private CommuterApplication(
            IMessageStore messageStore,
            IMessageQueue messageQueue,
            IMessagePump messagePump,
            IPushNotificationSubscription pushNotificationSubscription,
            IUserProxy userProxy)
            : base(
                  messageStore,
                  messageQueue,
                  messagePump,
                  pushNotificationSubscription,
                  userProxy)
        {
            _refreshTimer = new Timer(RefreshTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(5.0));
        }

        private void RefreshTick(object state)
        {
            if (Root?.SearchTerm?.IsBusy ?? false)
                SendAndReceiveMessages();
        }

        public static CommuterApplication LoadDesignModeApplication()
        {
            CommuterApplication application = new CommuterApplication();
            application.Load(new User(Guid.NewGuid()));
            return application;
        }

        public static CommuterApplication LoadApplication()
        {
            var secrets = new Secrets();

            string folderName = "Commuter";
            var store = new FileMessageStore(folderName);
            var queue = new FileMessageQueue(folderName);
            var bookmarkStore = new FileBookmarkStore(folderName);
            var push = new PushNotificationSubscription(
                secrets.NotificationHubPath,
                secrets.NotificationHubConnectionString);
            var accessTokenProvider = new CommuterAccessTokenProvider();
            var pump = new HttpMessagePump(
                new Uri(secrets.DistributorUrl, UriKind.Absolute),
                queue,
                bookmarkStore,
                accessTokenProvider,
                push);

            IUserProxy proxy = new HttpUserProxy(
                new Uri(secrets.UserIdentifierUrl, UriKind.Absolute),
                accessTokenProvider);
            var application = new CommuterApplication(
                store, queue, pump, push, proxy);

            application.Load(new User(Guid.NewGuid()));

            pump.Subscribe(() => application.Root
                .SearchTerm?.GetObjectId().ToCanonicalString());

            return application;
        }
    }
}
