using System;
using System.Collections.Immutable;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace Commuter.Search
{
    class SearchViewModel
    {
        private readonly CommuterApplication _application;
        private readonly SearchService _search;
        private readonly Subscriptions.SubscriptionService _subscription;
        private readonly Func<SearchResult, SearchResultViewModel> _newSearchResultViewModel;

        public SearchViewModel(
            CommuterApplication application,
            SearchService search,
            Subscriptions.SubscriptionService subscription,
            Func<SearchResult, SearchResultViewModel> newSearchResultViewModel)
        {
            _application = application;
            _search = search;
            _subscription = subscription;
            _newSearchResultViewModel = newSearchResultViewModel;
        }

        public string SearchTerm
        {
            get { return _search.SearchTerm; }
            set { _search.SearchTerm = value; }
        }

        public void QuerySubmitted()
        {
            _search.BeginSearch();
        }

        public string Heading => String.IsNullOrEmpty(_application.Root.SearchTerm?.Text)
            ? null
            : $"'{_application.Root.SearchTerm.Text}' Results";

        public void GoBack()
        {
            if (_search.SelectedSearchResult != null)
                _search.SelectedSearchResult = null;
            else
                _application.Root.ClearSearch();
        }

        public ImmutableList<SearchResultViewModel> SearchResults =>
            _application.Root.SearchTerm == null
                ? ImmutableList<SearchResultViewModel>.Empty
                :   (
                    from searchResult in _application.Root.SearchTerm.SearchResults
                    select _newSearchResultViewModel(searchResult)
                    ).ToImmutableList();

        public SearchResultViewModel SelectedSearchResult
        {
            get
            {
                return _search.SelectedSearchResult == null
                    ? null
                    : _newSearchResultViewModel(_search.SelectedSearchResult);
            }
            set
            {
                _search.SelectedSearchResult = value == null
                    ? null
                    : value.SearchResult;
            }
        }

        public bool HasSelectedSearchResult =>
            _application.Root.SearchTerm != null &&
            _application.Root.SearchTerm.SearchResults.Any() &&
            _search.SelectedSearchResult != null;

        public bool CanSubscribe =>
            _search.SelectedSearchResult != null &&
            !_subscription.IsSubscribed(_search.SelectedSearchResult.FeedUrl);

        public bool CanUnsubscribe =>
            _search.SelectedSearchResult != null &&
            _subscription.IsSubscribed(_search.SelectedSearchResult.FeedUrl);

        public void Subscribe()
        {
            if (_search.SelectedSearchResult != null)
            {
                _subscription.Subscribe(_search.SelectedSearchResult.FeedUrl);
                _subscription.ManagingSubscriptions = true;
            }
        }

        public void Unsubscribe()
        {
            if (_search.SelectedSearchResult != null)
            {
                _subscription.Unsubscribe(_search.SelectedSearchResult.FeedUrl);
            }
        }

        public string Message => ResourceLoader.GetForViewIndependentUse().GetString(
            (_application.Root.SearchTerm == null ||
             _application.Root.SearchTerm.IsBusy)
                ? "SearchBusy" :
            _search.SelectedSearchResult == null
                ? "SearchResults"
                : "SearchDetails");

        public string LastException =>
            _application.Exception?.Message ??
            _search.Exception?.Message;
    }
}
