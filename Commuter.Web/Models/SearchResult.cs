using System;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace Commuter.Web.Models
{
    public class SearchResult
    {
        private const string ITunesNamespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        public string feedUrl{ get; set; }
        public string title{ get; set; }
        public string subtitle{ get; set; }
        public string author{ get; set; }
        public string imageUri{ get; set; }

        public static async Task<SearchResult> TryLoadAsync(Uri feedUrl)
        {
            try
            {
                //var client = new SyndicationClient();
                //client.SetRequestHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

                var client = new HttpClient();
                var responseMessage = await client.GetAsync(feedUrl);
                if (responseMessage.IsSuccessStatusCode == false)
                    return null;

                var stream = await responseMessage.Content.ReadAsStreamAsync();
                using (var reader = XmlReader.Create(stream))
                {
                    var feed = SyndicationFeed.Load(reader);
                    var iTunesAuthor = GetITunesAttribute(feed, "author");
                    var iTunesSubtitle = GetITunesAttribute(feed, "subtitle");

                    var title = feed.Title.Text;
                    var author = iTunesAuthor ??
                        string.Join(", ", feed.Authors.Select(a => a.Name).ToArray());
                    var subtitle = iTunesSubtitle ??
                        feed.Description.Text;
                    var imageUri = feed.ImageUrl;

                    return new SearchResult
                    {
                        feedUrl = feedUrl.ToString(),
                        title = title,
                        author = author,
                        subtitle = subtitle,
                        imageUri = imageUri.ToString()
                    };
                }
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
                    x.OuterNamespace == ITunesNamespace &&
                    x.OuterName == attribute)
                .Select(x => x.GetObject<string>())
                .FirstOrDefault();
        }
    }
}