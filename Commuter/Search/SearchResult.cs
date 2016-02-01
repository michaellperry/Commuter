using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Search
{
    class SearchResult
    {
        private readonly string _title;
        private readonly Uri _feedUrl;

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
    }
}
