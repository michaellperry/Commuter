using Newtonsoft.Json.Linq;
using RoverMob.Messaging;
using RoverMob.Protocol;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Commuter.Search
{
    public class SearchResult
    {
        private const string ITunesNamespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        private readonly Uri _feedUrl;
        private readonly string _title;
        private readonly string _subtitle;
        private readonly string _author;
        private readonly Uri _imageUri;
        private readonly MessageHash _hash;

        private SearchResult(
            Uri feedUrl,
            string title,
            string subtitle,
            string author,
            Uri imageUri,
            MessageHash hash)
        {
            _feedUrl = feedUrl;
            _title = title;
            _subtitle = subtitle;
            _author = author;
            _imageUri = imageUri;
            _hash = hash;
        }

        public string Title => _title;
        public Uri FeedUrl => _feedUrl;
        public string Author => _author;
        public string Subtitle => _subtitle;
        public Uri ImageUri => _imageUri;
        public MessageHash Hash => _hash;

        public static SearchResult FromMessage(Message message)
        {
            string feedUrl = message.Body.FeedUrl;
            string title = message.Body.Title;
            string subtitle = message.Body.Subtitle;
            string author = message.Body.Author;
            string imageUri = message.Body.ImageUri;

            var searchResult = new SearchResult(
                new Uri(feedUrl, UriKind.Absolute),
                title,
                subtitle,
                author,
                new Uri(imageUri, UriKind.Absolute),
                message.Hash);
            return searchResult;
        }

        public static SearchResult FromJson(JObject obj)
        {
            return new SearchResult(
                new Uri((string)obj["feedUrl"], UriKind.Absolute),
                (string)obj["title"],
                (string)obj["subtitle"],
                (string)obj["author"],
                new Uri((string)obj["imageUri"], UriKind.Absolute),
                null);
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

                return new SearchResult(feedUrl, title, subtitle, author, imageUri, null);
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
