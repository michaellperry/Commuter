using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Commuter.Details
{
    class DetailViewModel
    {
        private readonly Podcast _podcast;
        private readonly Func<Episode, EpisodeViewModel> _newEpisodeViewModel;

        public DetailViewModel(
            Podcast podcast,
            Func<Episode, EpisodeViewModel> newEpisodeViewModel)
        {
            _podcast = podcast;
            _newEpisodeViewModel = newEpisodeViewModel;
        }

        public string Title => _podcast.Title;
        public string Subtitle => _podcast.Subtitle;
        public string Author => _podcast.Author;
        public Uri ImageUri => _podcast.ImageUri;
        public ImmutableList<EpisodeViewModel> Episodes =>
            (from episode in _podcast.Episodes
             orderby episode.PublishDate descending
             select _newEpisodeViewModel(episode)).ToImmutableList();
    }
}