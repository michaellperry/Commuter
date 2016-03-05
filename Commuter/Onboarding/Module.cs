using Autofac;

namespace Commuter.Onboarding
{
    class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginViewModel>()
                .AsSelf();
            builder.RegisterType<OnboardingViewModel>()
                .AsSelf();
        }
    }
}