using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Commuter.Search
{
    class SearchResult
    {
        private const string ITunesNamespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        private readonly Uri _feedUrl;
        private readonly string _title;
        private readonly string _subtitle;
        private readonly string _author;
        private readonly Uri _imageUri;

        private SearchResult(
            Uri feedUrl,
            string title,
            string subtitle,
            string author,
            Uri imageUri)
        {
            _feedUrl = feedUrl;
            _title = title;
            _subtitle = subtitle;
            _author = author;
            _imageUri = imageUri;
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
            get { return _imageUri; }
        }

        public static async Task<SearchResult> TryLoadAsync(Uri feedUrl)
        {
            try
            {
                var client = new SyndicationClient();
                client.SetRequestHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

                var feed = await client.RetrieveFeedAsync(feedUrl);
                var iTunesAuthor = GetITunesAttribute(feed, "author");
                var iTunesSubtitle = GetITunesAttribute(feed, "subtitle");

                var title = feed.Title.Text;
                var author = iTunesAuthor ??
                    string.Join(", ", feed.Authors.Select(a => a.Name).ToArray());
                var subtitle = iTunesSubtitle ??
                    feed.Subtitle.Text;
                var imageUri = feed.ImageUri;

                return new SearchResult(feedUrl, title, subtitle, author, imageUri);
            }
            catch (Exception x)
            {
                return null;
            }
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
