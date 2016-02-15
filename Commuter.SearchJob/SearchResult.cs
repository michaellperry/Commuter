using System;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace Commuter.SearchJob
{
    public class SearchResult
    {
        private const string ITunesNamespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        public string FeedUrl { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Author { get; set; }
        public string ImageUri { get; set; }

        public static async Task<SearchResult> TryLoadAsync(Uri feedUrl)
        {
            try
            {
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
                        FeedUrl = feedUrl.ToString(),
                        Title = title,
                        Author = author,
                        Subtitle = subtitle,
                        ImageUri = imageUri.ToString()
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