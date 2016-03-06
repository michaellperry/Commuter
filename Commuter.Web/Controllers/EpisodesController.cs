using Commuter.DigitalPodcast;
using Commuter.Web.Models;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace Commuter.Web.Controllers
{
    public class EpisodesController : ApiController
    {
        // api/episodes?feed={url}
        public async Task<IHttpActionResult> Get(string feed)
        {
            if (string.IsNullOrEmpty(feed))
                return NotFound();

            var episodeList = await TryLoadAsync(new Uri(feed, UriKind.Absolute));
            if (episodeList == null)
                return NotFound();

            return Ok(episodeList);
        }

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
                        PublishDate = i.PublishDate.DateTime
                    });

                return new EpisodeList
                {
                    Episodes = episodes.ToImmutableList()
                };
            }
        }
    }
}
