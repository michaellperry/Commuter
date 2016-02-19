using Newtonsoft.Json.Linq;
using RoverMob.Messaging;
using System;

namespace Commuter.Search
{
    public class SearchResult
    {
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
    }
}
