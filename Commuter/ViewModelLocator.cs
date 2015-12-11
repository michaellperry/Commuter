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
            builder.RegisterType<Model>()
                .SingleInstance()
                .AsSelf();
            builder.RegisterType<Onboarding.OnboardingViewModel>()
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
    }
}
