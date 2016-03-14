using Commuter.PodcastFeed;
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

            var episodeList = await PodcastFunctions.TryLoadAsync(
                new Uri(feed, UriKind.Absolute));
            if (episodeList == null)
                return NotFound();

            return Ok(episodeList);
        }
    }
}
