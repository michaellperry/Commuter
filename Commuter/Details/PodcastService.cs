using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoverMob.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Commuter.Details
{
    class PodcastService : Process
    {
        public void BeginLoadPodcast(Podcast podcast)
        {
            Perform(() => LoadPodcastAsync(podcast));
        }

        private async Task LoadPodcastAsync(Podcast podcast)
        {
            await Task.Delay(500);
            podcast.SetEpisodes(new Episode[]
            {
                new Episode
                {
                    Title = "QED 11: The one before Difference Engine",
                    PublishDate = new DateTime(2015, 6, 19)
                },
                new Episode
                {
                    Title = "QED 12: Difference Engine",
                    PublishDate = new DateTime(2015, 7, 19)
                }
            });
        }

        private static async Task<JObject> GetJsonAsync(string requestUri)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(
                    "application/json"));
                var responseMessage = await client.SendAsync(request);
                if (responseMessage.IsSuccessStatusCode == false)
                    throw new InvalidOperationException(responseMessage.ReasonPhrase);

                using (var stream = await responseMessage.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return (JObject)JToken.ReadFrom(jsonReader);
                }
            }
        }
    }
}
