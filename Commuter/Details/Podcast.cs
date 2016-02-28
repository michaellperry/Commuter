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

        public async Task LoadAsync()
        {
            await Task.Delay(500);
            _episodes.Clear();
            _episodes.Add(new Episode
            {
                Title = "QED 11: The one before Difference Engine",
                PublishDate = new DateTime(2015, 6, 19)
            });
            _episodes.Add(new Episode
            {
                Title = "QED 12: Difference Engine",
                PublishDate = new DateTime(2015, 7, 19)
            });
        }

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
            podcast.LoadAsync();
            return podcast;
        }
    }
}