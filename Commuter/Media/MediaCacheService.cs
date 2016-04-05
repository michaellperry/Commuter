using Assisticant.Collections;
using Assisticant.Fields;
using Commuter.MyCommute;
using RoverMob.Tasks;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Commuter.Media
{
    class MediaCacheService : Process, IDisposable
    {
        private readonly ComputedSubscription _subscribeToStart;

        public MediaCacheService(
            CommuterApplication application,
            Func<Queue, MediaDownloader> createMediaDownloader)
        {
            var mediaDownloaders = new ComputedList<MediaDownloader>(() =>
                application.Root?.QueuedEpisodes.Select(e =>
                    createMediaDownloader(e))
                    .ToImmutableList() ?? ImmutableList<MediaDownloader>.Empty);
            var dowloadersToStart = new Computed<ImmutableList<MediaDownloader>>(() =>
                mediaDownloaders.Where(d => d.ShouldStart).ToImmutableList());
            _subscribeToStart = dowloadersToStart.Subscribe(StartDownloaders);
        }

        public void Dispose()
        {
            _subscribeToStart.Unsubscribe();
        }

        private void StartDownloaders(ImmutableList<MediaDownloader> downloaders)
        {
            foreach (var downloader in downloaders)
                Perform(() => downloader.Start());
        }
    }
}
