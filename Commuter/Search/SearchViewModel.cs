using System;
using System.Collections.Immutable;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace Commuter.Search
{
    class SearchViewModel
    {
        private readonly SearchService _search;
        private readonly Subscriptions.SubscriptionService _subscription;
        private readonly Func<SearchResult, SearchResultViewModel> _newSearchResultViewModel;

        public SearchViewModel(
            SearchService search,
            Subscriptions.SubscriptionService subscription,
            Func<SearchResult, SearchResultViewModel> newSearchResultViewModel)
        {
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

        public string Heading => String.IsNullOrEmpty(_search.SearchResultTerm)
            ? null
            : $"'{_search.SearchResultTerm}' Results";

        public void GoBack()
        {
            if (_search.SelectedSearchResult != null)
                _search.SelectedSearchResult = null;
            else
                _search.ClearSearchResults();
        }

        public ImmutableList<SearchResultViewModel> SearchResults =>
            (
            from searchResult in _search.SearchResults
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
            !_search.Busy &&
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
            _search.Busy
                ? "SearchBusy" :
            _search.SelectedSearchResult == null
                ? "SearchResults"
                : "SearchDetails");
    }
}
