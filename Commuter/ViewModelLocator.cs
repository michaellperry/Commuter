using Assisticant;
using Autofac;
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
            builder.RegisterModule<Details.Module>();
            builder.RegisterModule<Images.Module>();
            builder.RegisterType<Model>()
                .SingleInstance()
                .AsSelf();
            builder.RegisterType<Onboarding.OnboardingViewModel>()
                .AsSelf();
            builder.RegisterInstance(DesignMode
                ? CommuterApplication.LoadDesignModeApplication()
                : CommuterApplication.LoadApplication())
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

        public Model Model => _container.Resolve<Model>();

        public object LoginViewModel => ViewModel(() =>
            _container.Resolve<Onboarding.LoginViewModel>());

        public object OnboardingViewModel => ViewModel(() =>
            _container.Resolve<Onboarding.OnboardingViewModel>());

        public object SearchViewModel => ViewModel(() =>
            _container.Resolve<Search.SearchViewModel>());

        public object MyCommuteViewModel => ViewModel(() =>
            _container.Resolve<MyCommute.MyCommuteViewModel>());

        public object SubscriptionViewModel => ViewModel(() =>
            _container.Resolve<Subscriptions.SubscriptionViewModel>());
    }
}
