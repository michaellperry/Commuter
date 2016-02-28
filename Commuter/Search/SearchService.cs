using Assisticant.Collections;
using Assisticant.Fields;
using RoverMob.Messaging;
using System;

namespace Commuter.Search
{
    class SearchService
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
    }
}
