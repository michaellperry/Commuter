using Commuter.ITunes;
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
                    var iTunesAuthor = feed.GetITunesAttribute("author");
                    var iTunesSubtitle = feed.GetITunesAttribute("subtitle");

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
            catch (Exception)
            {
                return null;
            }
        }
    }
}