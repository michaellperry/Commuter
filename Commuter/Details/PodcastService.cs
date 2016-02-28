using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.IO;
using RoverMob.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Networking.Connectivity;

namespace Commuter.Details
{
    class PodcastService : Process
    {
        private JsonSerializer _serializer = new JsonSerializer();

        public void BeginLoadPodcast(Podcast podcast)
        {
            Perform(() => LoadPodcastAsync(podcast));
        }

        private async Task LoadPodcastAsync(Podcast podcast)
        {
            ImmutableList<Episode> episodes;
            if (NetworkInformation.GetInternetConnectionProfile().GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                episodes = await LoadEpisodesFromServerAsync(podcast.FeedUrl);
                await SaveEpisodesToCache(podcast.FeedUrl, episodes);
            }
            else
            {
                episodes = await LoadEpisodesFromCache(podcast.FeedUrl);
            }
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

        private async Task SaveEpisodesToCache(Uri feedUrl, ImmutableList<Episode> episodes)
        {
            string fileName = GetFileName(feedUrl);
            var episodesFolder = await OpenEpisodesFolder();

            var file = await episodesFolder
                .CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var outputStream = await file.OpenStreamForWriteAsync();
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter(outputStream)))
            {
                _serializer.Serialize(writer, episodes);
            }
        }

        private async Task<ImmutableList<Episode>> LoadEpisodesFromCache(Uri feedUrl)
        {
            string fileName = GetFileName(feedUrl);
            var episodesFolder = await OpenEpisodesFolder();

            var file = await episodesFolder
                .CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            var inputStream = await file.OpenStreamForReadAsync();
            using (JsonReader reader = new JsonTextReader(new StreamReader(inputStream)))
            {
                var episodeList = _serializer.Deserialize<List<Episode>>(reader);
                return episodeList.ToImmutableList();
            }
        }

        private static string GetFileName(Uri feedUrl)
        {
            string uri = feedUrl.ToString();
            var sha = new Sha256Digest();
            var stream = new DigestStream(new MemoryStream(), null, sha);
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(uri);
            }
            byte[] buffer = new byte[sha.GetDigestSize()];
            sha.DoFinal(buffer, 0);
            string hex = BitConverter.ToString(buffer);
            string fileName = hex.Replace("-", "") + ".json";
            return fileName;
        }

        private static async Task<StorageFolder> OpenEpisodesFolder()
        {
            var episodesFolder = await ApplicationData.Current.LocalFolder
                .CreateFolderAsync("Episodes", CreationCollisionOption.OpenIfExists);
            return episodesFolder;
        }
    }
}
