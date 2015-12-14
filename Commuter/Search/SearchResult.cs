using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Search
{
    class SearchResult
    {
        private readonly float _quality = 1.0f;
        private readonly Uri _feedUrl = RandomFeedUrl();

        public float Quality
        {
            get { return _quality; }
        }

        public Uri FeedUrl
        {
            get { return _feedUrl; }
        }


        private static Random _rand = new Random();

        private static Uri RandomFeedUrl()
        {
            return new Uri($"http://qedcode.libsyn.com/rss/{_rand.Next(10000)}", UriKind.Absolute);
        }
    }
}
