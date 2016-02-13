using System;
using System.Collections.Generic;

namespace Commuter.Web.Models
{
    public class SearchResponse
    {
        public IEnumerable<SearchResult> results { get; set; }
    }
}