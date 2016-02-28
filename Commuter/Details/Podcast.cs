using Assisticant.Collections;
using System;
using System.Collections.Generic;

namespace Commuter.Details
{
    class Podcast
    {
        private ObservableList<Episode> _episodes = new ObservableList<Episode>();

        public string Author { get; set; }
        public Uri FeedUrl { get; set; }
        public Uri ImageUri { get; set; }
        public string Subtitle { get; set; }
        public string Title { get; set; }
        public IEnumerable<Episode> Episodes => _episodes;

        public void SetEpisodes(IEnumerable<Episode> episodes)
        {
            _episodes.Clear();
            _episodes.AddRange(episodes);
        }
    }
}