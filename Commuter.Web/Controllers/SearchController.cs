using Commuter.DigitalPodcast;
using Commuter.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Commuter.Web.Controllers
{
    public class SearchController : ApiController
    {
        // GET api/search/{keyword}
        public async Task<SearchResponse> Get(string id)
        {
            string searchTerm = id;

            var results = new SearchResult[0];

            return new SearchResponse
            {
                results = results
            };
        }
    }
}
