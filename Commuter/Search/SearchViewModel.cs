using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void GoBack()
        {
            if (_search.SelectedSearchResult != null)
                _search.SelectedSearchResult = null;
            else
                _search.ClearSearchResults();
        }

        public ImmutableList<SearchResultViewModel> SearchResults
        {
            get
            {
                return (
                    from searchResult in _search.SearchResults
                    orderby searchResult.Quality descending
                    select _newSearchResultViewModel(searchResult)
                    ).ToImmutableList();
            }
        }

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

        public bool HasSelectedSearchResult
        {
            get { return _search.SelectedSearchResult != null; }
        }

        public void Subscribe()
        {
            if (_search.SelectedSearchResult != null)
            {
                _subscription.Subscribe(_search.SelectedSearchResult.FeedUrl);
            }
        }
    }
}
