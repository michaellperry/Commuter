using Commuter.DigitalPodcast;
using Commuter.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Commuter.Web.Controllers
{
    public class SearchController : ApiController
    {
        // GET api/search/{keyword}
        public async Task<SearchResponse> Get(string id)
        {
            string searchTerm = id;

            var search = new DigitalPodcastSearch(
                new Secrets().DigitalPodcastApiKey);
            var response = await search.SearchAsync(
                new DigitalPodcastRequest
                {
                    Keywords = searchTerm
                });
            var tasks = response.Results
                .Select(r => SearchResult.TryLoadAsync(r.FeedUrl));
            var allResults = await Task.WhenAll(tasks);
            var results = allResults
                .Where(r => r != null);

            return new SearchResponse
            {
                results = results
            };
        }
    }
}
