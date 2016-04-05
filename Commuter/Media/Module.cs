using Autofac;

namespace Commuter.Media
{
    class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MediaCacheService>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<MediaDownloader>()
                .AsSelf();
        }
    }
}
