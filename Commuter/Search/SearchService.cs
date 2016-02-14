using Assisticant.Collections;
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
        private Observable<string> _searchResultTerm = new Observable<string>();
        private ObservableList<SearchResult> _searchResults = new ObservableList<SearchResult>();
        private Observable<SearchResult> _selectedSearchResult = new Observable<SearchResult>();

        public SearchService(CommuterApplication application)
        {
            _application = application;
        }

        public string SearchTerm
        {
            get { return _searchTerm.Value; }
            set { _searchTerm.Value = value; }
        }

        public string SearchResultTerm
        {
            get { return _searchResultTerm.Value; }
        }

        public ImmutableList<SearchResult> SearchResults
        {
            get { return _searchResults.ToImmutableList(); }
        }

        public SearchResult SelectedSearchResult
        {
            get { return _selectedSearchResult.Value; }
            set { _selectedSearchResult.Value = value; }
        }

        public void BeginSearch()
        {
            string searchTerm = _searchTerm.Value;

            var searchTermBody = new { Text = searchTerm };
            var searchTermMessage = Message.CreateMessage(
                "search",
                "SearchTerm",
                searchTermBody.ToGuid(),
                searchTermBody);
            _application.EmitMessage(searchTermMessage);
            _application.EmitMessage(Message.CreateMessage(
                "search",
                "Search",
                Predecessors.Set
                    .In("SearchTerm", searchTermMessage.Hash),
                Guid.NewGuid(),
                new
                {
                    Time = DateTime.UtcNow
                }));

            _searchResultTerm.Value = searchTerm;
            _searchResults.Clear();
            _selectedSearchResult.Value = null;
            _application.AddSubscription(
                searchTermMessage.ObjectId.ToCanonicalString());

            Perform(async delegate
            {
                var root = await GetJsonAsync(
                    $"http://commuterweb.azurewebsites.net/api/search/{searchTerm}");
                var results = root["results"].OfType<JObject>()
                    .Select(j => SearchResult.FromJson(j));

                _searchResults.AddRange(results);
            });
        }

        public void ClearSearchResults()
        {
            _searchTerm.Value = null;
            _searchResultTerm.Value = null;
            _searchResults.Clear();
            _selectedSearchResult.Value = null;
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
