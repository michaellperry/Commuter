using Autofac;

namespace Commuter.MyCommute
{
    class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MyCommuteViewModel>()
                .AsSelf();
        }
    }
}