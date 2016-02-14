using Commuter.Subscriptions;
using RoverMob;
using RoverMob.Messaging;
using System;

namespace Commuter
{
    public class CommuterApplication : Application<User>
    {
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

        }

        public static CommuterApplication LoadDesignModeApplication()
        {
            CommuterApplication application = new CommuterApplication();
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

            return application;
        }

    }
}
