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

namespace Commuter.Search
{
    class SearchService : RoverMob.Tasks.Process
    {
        private Observable<string> _searchTerm = new Observable<string>();
        private Observable<string> _searchResultTerm = new Observable<string>();
        private ObservableList<SearchResult> _searchResults = new ObservableList<SearchResult>();
        private Observable<SearchResult> _selectedSearchResult = new Observable<SearchResult>();

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
            Perform(async delegate
            {
                string searchTerm = _searchTerm.Value;

                var client = new HttpClient();
                var responseMessage = await client.GetAsync(
                    $"http://localhost:2648/api/search/{searchTerm}");
                if (responseMessage.IsSuccessStatusCode == false)
                    throw new InvalidOperationException(responseMessage.ReasonPhrase);

                var stream = await responseMessage.Content.ReadAsStreamAsync();
                var reader = new StreamReader(stream);
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var token = JToken.ReadFrom(jsonReader);
                    var results = token["results"].OfType<JObject>()
                        .Select(j => SearchResult.FromJson(j));

                    _searchResultTerm.Value = searchTerm;
                    _searchResults.Clear();
                    _searchResults.AddRange(results);
                }
            });
        }

        public void ClearSearchResults()
        {
            _searchTerm.Value = null;
            _searchResultTerm.Value = null;
            _searchResults.Clear();
            _selectedSearchResult.Value = null;
        }
    }
}
