using Commuter.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Commuter.Web.Controllers
{
    [Authorize]
    public class SearchController : ApiController
    {
        // GET api/search/{keyword}
        public SearchResponse Get(int id)
        {
            return new SearchResponse
            {
            };
        }
    }
}
