using Assisticant;
using Autofac;
using Commuter.Subscriptions;
using RoverMob;
using RoverMob.Messaging;
using System;

namespace Commuter
{
    class ViewModelLocator : ViewModelLocatorBase, IDisposable
    {
        private IContainer _container;

        public ViewModelLocator()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<Onboarding.Module>();
            builder.RegisterModule<Search.Module>();
            builder.RegisterModule<MyCommute.Module>();
            builder.RegisterModule<Subscriptions.Module>();
            builder.RegisterType<Model>()
                .SingleInstance()
                .AsSelf();
            builder.RegisterType<Onboarding.OnboardingViewModel>()
                .AsSelf();
            builder.RegisterInstance(DesignMode
                ? LoadDesignModeApplication()
                : LoadApplication())
                .AsSelf();
            _container = builder.Build();
        }

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }
        }

        public Model Model
        {
            get { return _container.Resolve<Model>(); }
        }

        public object OnboardingViewModel
        {
            get
            {
                return ViewModel(() =>
                    _container.Resolve<Onboarding.OnboardingViewModel>());
            }
        }

        public object SearchViewModel
        {
            get
            {
                return ViewModel(() =>
                    _container.Resolve<Search.SearchViewModel>());
            }
        }

        public object MyCommuteViewModel
        {
            get
            {
                return ViewModel(() =>
                    _container.Resolve<MyCommute.MyCommuteViewModel>());
            }
        }

        public object SubscriptionViewModel
        {
            get
            {
                return ViewModel(() =>
                    _container.Resolve<Subscriptions.SubscriptionViewModel>());
            }
        }

        private Application<User> LoadDesignModeApplication()
        {
            Application<User> application = new Application<User>();
            return application;
        }

        private Application<User> LoadApplication()
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
                new Uri("http://fieldservicedistributor.azurewebsites.net/api/technicianidentifier/", UriKind.Absolute),
                accessTokenProvider);
            var application = new Application<User>(
                store, queue, pump, push, proxy);

            return application;
        }
    }
}
