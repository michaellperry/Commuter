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

        public SearchTerm ActiveSearchTerm => _application.Root?.SearchTerm;

        public SearchResult SelectedSearchResult
        {
            get { return _application.Root?.SelectedSearchResult; }
            set
            {
                if (_application.Root != null)
                    _application.Root.SelectedSearchResult = value;
            }
        }

        public void ClearSearchResults()
        {
            _searchTerm.Value = null;
            if (_application.Root != null)
                _application.Root.SelectedSearchResult = null;
        }

        public void ClearSearch()
        {
            if (_application.Root != null)
                _application.Root.ClearSearch();
        }
    }
}
