using Autofac;

namespace Commuter.Search
{
    class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SearchService>()
                .SingleInstance()
                .AsSelf();
            builder.RegisterType<SearchViewModel>()
                .AsSelf();
            builder.RegisterType<SearchResultViewModel>()
                .AsSelf();
        }
    }
}
