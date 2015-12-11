using Autofac;

namespace Commuter.Subscriptions
{
    class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SubscriptionService>()
                .SingleInstance()
                .AsSelf();
            builder.RegisterType<SubscriptionViewModel>()
                .AsSelf();
        }
    }
}