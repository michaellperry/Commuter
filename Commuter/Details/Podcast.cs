using Assisticant.Collections;
using Commuter.Search;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public static Podcast FromSearchResult(SearchResult searchResult)
        {
            var podcast = new Podcast
            {
                Title = searchResult.Title,
                Subtitle = searchResult.Subtitle,
                Author = searchResult.Author,
                FeedUrl = searchResult.FeedUrl,
                ImageUri = searchResult.ImageUri
            };
            return podcast;
        }

        public void SetEpisodes(IEnumerable<Episode> episodes)
        {
            _episodes.Clear();
            _episodes.AddRange(episodes);
        }
    }
}