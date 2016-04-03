using System.Linq;
using System.ServiceModel.Syndication;

namespace Commuter.ITunes
{
    public static class ITunesExtentions
    {
        private const string ITunesNamespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        public static string GetITunesAttribute(this SyndicationFeed feed, string attribute)
        {
            return feed.ElementExtensions
                .Where(x =>
                    x.OuterNamespace == ITunesNamespace &&
                    x.OuterName == attribute)
                .Select(x => x.GetObject<string>())
                .FirstOrDefault();
        }

        public static string GetITunesAttribute(this SyndicationItem item, string attribute)
        {
            return item.ElementExtensions
                .Where(x =>
                    x.OuterNamespace == ITunesNamespace &&
                    x.OuterName == attribute)
                .Select(x => x.GetObject<string>())
                .FirstOrDefault();
        }
    }
}
