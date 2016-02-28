using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoverMob.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            var episodes = await LoadEpisodesFromServerAsync(podcast.FeedUrl);
            podcast.SetEpisodes(episodes);
        }

        private static async Task<ImmutableList<Episode>> LoadEpisodesFromServerAsync(Uri feedUrl)
        {
            string requestUri = $"{new Secrets().EpisodesUrl}?feed={Uri.EscapeUriString(feedUrl.ToString())}";
            var root = await GetJsonAsync(requestUri);

            var episodes = root["Episodes"].OfType<JObject>()
                .Select(j => new Episode
                {
                    Title = j["Title"].Value<string>(),
                    PublishDate = j["PublishDate"].Value<DateTime>()
                });
            return episodes.ToImmutableList();
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
