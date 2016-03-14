using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace Commuter.PodcastFeed
{
    public static class PodcastFunctions
    {
        public static async Task<EpisodeList> TryLoadAsync(Uri feedUrl)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(feedUrl);
            if (responseMessage.IsSuccessStatusCode == false)
                return null;

            var stream = await responseMessage.Content.ReadAsStreamAsync();
            using (var reader = XmlReader.Create(stream))
            {
                var feed = SyndicationFeed.Load(reader);

                var episodes = feed.Items
                    .Select(i => new Episode
                    {
                        Title = i.Title.Text,
                        Summary = HtmlParser.ContentOfHtml(i.Summary.Text),
                        PublishDate = i.PublishDate.DateTime,
                        MediaUrl = i.Links
                            .Where(l => l.RelationshipType == "enclosure")
                            .Select(l => l.Uri.ToString())
                            .FirstOrDefault()
                    });

                return new EpisodeList
                {
                    Episodes = episodes.ToImmutableList()
                };
            }
        }
    }
}
