using Assisticant.Fields;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Commuter.Search
{
    class SearchResult
    {
        private const string ITunesNamespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";
        private readonly string _title;
        private readonly Uri _feedUrl;

        private Observable<string> _author = new Observable<string>();
        private Observable<string> _subtitle = new Observable<string>();
        private Observable<Uri> _imageUri = new Observable<Uri>();

        public SearchResult(string title, Uri feedUrl)
        {
            _title = title;
            _feedUrl = feedUrl;
        }

        public string Title
        {
            get { return _title; }
        }

        public Uri FeedUrl
        {
            get { return _feedUrl; }
        }

        public string Author
        {
            get { return _author; }
        }

        public string Subtitle
        {
            get { return _subtitle; }
        }

        public Uri ImageUri
        {
            get { return _imageUri.Value; }
        }

        public async Task LoadAsync()
        {
            var client = new SyndicationClient();
            client.SetRequestHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

            var feed = await client.RetrieveFeedAsync(_feedUrl);
            var iTunesAuthor = GetITunesAttribute(feed, "author");
            var iTunesSubtitle = GetITunesAttribute(feed, "subtitle");

            _author.Value = iTunesAuthor ??
                string.Join(", ", feed.Authors.Select(a => a.Name).ToArray());
            _subtitle.Value = iTunesSubtitle ??
                feed.Subtitle.Text;
            _imageUri.Value = feed.ImageUri;
        }

        private static string GetITunesAttribute(SyndicationFeed feed, string attribute)
        {
            return feed.ElementExtensions
                .Where(x =>
                    x.NodeNamespace == ITunesNamespace &&
                    x.NodeName == attribute)
                .Select(x => x.NodeValue)
                .FirstOrDefault();
        }
    }
}
