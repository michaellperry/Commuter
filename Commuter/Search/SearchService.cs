﻿using Assisticant.Collections;
using Assisticant.Fields;
using Commuter.DigitalPodcast;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using RoverMob.Messaging;
using RoverMob;

namespace Commuter.Search
{
    class SearchService : RoverMob.Tasks.Process
    {
        private readonly CommuterApplication _application;

        private Observable<string> _searchTerm = new Observable<string>();
        private ObservableList<SearchResult> _searchResults = new ObservableList<SearchResult>();

        public SearchService(CommuterApplication application)
        {
            _application = application;
        }

        public string SearchTerm
        {
            get { return _searchTerm.Value; }
            set { _searchTerm.Value = value; }
        }

        public void BeginSearch()
        {
            string searchTerm = _searchTerm.Value;

            _application.EmitMessage(Message.CreateMessage(
                "search",
                "Search",
                _application.Root.GetObjectId(),
                new
                {
                    SearchTerm = searchTerm,
                    Time = DateTime.UtcNow
                }));
        }

        public void ClearSearchResults()
        {
            _searchTerm.Value = null;
            _searchResults.Clear();
            _application.Root.SelectedSearchResult = null;
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
