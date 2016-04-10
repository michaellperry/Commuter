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

            application.EmitMessage(Message.CreateMessage(
                application.Root.GetObjectId().ToCanonicalString(),
                "Queue",
                application.Root.GetObjectId(),
                new
                {
                    Title = "QED 12: Difference Engine",
                    Summary = @"The Difference Engine was a mechanical computer that could calculate tables of numbers based on polynomials. The amazing thing is, though, that it could only add. How then could it accomplish this feat? By the method of differences! Charles Babbage never constructed his Difference Engine, but we've made a couple from his designs.
Lambda Calculus is also a method of computation based on really simple rules. In this case, they are alpha-conversion, beta-conversion, and eta-conversion. These serious-sounding transforms are actually pretty simple. Let's first learn what they are, and then see how they relate to C#.
Speaking of C#, Malachi asks a question about C# constructors. As you may know, I am of the opinion that constructor parameters should represent only immutable fields. How, then, does one initialize a mutable field in such a way that you can guarantee that it is set? In other words, write a method on the class that can only be called after the object is initialized.",
                    PublishedDate = DateTime.Parse("7/19/2015"),
                    MediaUrl = "http://traffic.libsyn.com/qedcode/qed12-DifferenceEngine.mp3",
                    ImageUri = "http://assets.libsyn.com/content/7569532"
                }));
            application.EmitMessage(Message.CreateMessage(
                null,
                "Downloaded",
                new { UserGuid = application.Root.GetObjectId(), mediaUrl = "http://traffic.libsyn.com/qedcode/qed12-DifferenceEngine.mp3" }.ToGuid(),
                new
                {
                    FileName = @"c:\Fake\File\Path.mp3"
                }));

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
            var accessTokenProvider = new WebAuthenticationBrokerAccessTokenProvider(
                "https://commuterweb.azurewebsites.net",
                "User",
                "/LoggedIn");
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

            application.GetUserIdentifier("User", guid =>
                application.Load(new User(guid)));

            pump.Subscribe(() => application.Root
                ?.GetObjectId().ToCanonicalString());
            pump.Subscribe(() => application.Root
                ?.SearchTerm?.GetObjectId().ToCanonicalString());
            pump.Subscribe(() => application.Root
                ?.Subscriptions.Select(s => s.GetObjectId().ToCanonicalString()));

            return application;
        }
    }
}
