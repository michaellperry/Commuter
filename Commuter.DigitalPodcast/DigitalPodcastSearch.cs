using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Xml.Linq;

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
            HttpResponseMessage httpResponse = await client.GetAsync(string.Format("search/?appid={0}&keywords={1}&format=rssopml", _apiKey, request.Keywords));
            if (httpResponse.IsSuccessStatusCode)
            {
                using (var stream = await httpResponse.Content.ReadAsStreamAsync())
                    return Opml.Parse(XDocument.Load(stream));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
