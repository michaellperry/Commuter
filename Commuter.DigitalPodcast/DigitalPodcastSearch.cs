﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Commuter.DigitalPodcast
{
    public class DigitalPodcastSearch
    {
        private readonly string _apiKey;

        public DigitalPodcastSearch(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<DigitalPodcastResponse> SearchAsync(DigitalPodcastRequest request)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://api.digitalpodcast.com/v2r/", UriKind.Absolute);
            HttpResponseMessage httpResponse = await client.GetAsync("search/?appid=43944b9fe5bf600bc2efae0e5904f5f5&keywords=sharepoint&format=rssopml");
            if (httpResponse.IsSuccessStatusCode)
            {
                var serializer = new DataContractSerializer(typeof(Opml.OpmlResponse));
                using (var stream = await httpResponse.Content.ReadAsStreamAsync())
                {
                    var opmlResponse = (Opml.OpmlResponse)serializer.ReadObject(stream);
                }
                ImmutableList<DigitalPodcastResult> results = null;
                return new DigitalPodcastResponse
                {
                    Results = results
                };
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
