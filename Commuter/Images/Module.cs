using Autofac;

namespace Commuter.Images
{
    class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageCacheService>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
