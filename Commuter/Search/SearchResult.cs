using Newtonsoft.Json.Linq;
using System;

namespace Commuter.Search
{
    class SearchResult
    {
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

        public static SearchResult FromJson(JObject obj)
        {
            return new SearchResult(
                new Uri((string)obj["feedUrl"], UriKind.Absolute),
                (string)obj["title"],
                (string)obj["subtitle"],
                (string)obj["author"],
                new Uri((string)obj["imageUri"], UriKind.Absolute));
        }
    }
}
