using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Commuter.Details
{
    class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PodcastService>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<DetailViewModel>()
                .AsSelf();
            builder.RegisterType<EpisodeViewModel>()
                .AsSelf();
        }
    }
}
