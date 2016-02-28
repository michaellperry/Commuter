using Commuter.Details;
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
        private readonly Func<Podcast, DetailViewModel> _newDetailViewModel;
        
        public SearchViewModel(
            CommuterApplication application,
            SearchService search,
            Subscriptions.SubscriptionService subscription,
            Func<SearchResult, SearchResultViewModel> newSearchResultViewModel,
            Func<Podcast, DetailViewModel> newDetailViewModel)
        {
            _application = application;
            _search = search;
            _subscription = subscription;
            _newSearchResultViewModel = newSearchResultViewModel;
            _newDetailViewModel = newDetailViewModel;
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
            if (_application.Root.SelectedSearchResult != null)
                _application.Root.SelectedSearchResult = null;
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
                return _application.Root.SelectedSearchResult == null
                    ? null
                    : _newSearchResultViewModel(_application.Root.SelectedSearchResult);
            }
            set
            {
                _application.Root.SelectedSearchResult = value == null
                    ? null
                    : value.SearchResult;
            }
        }

        public DetailViewModel SelectedPodcastDetail =>
            _application.Root.SelectedSearchResult == null
                ? null
                : _newDetailViewModel(Podcast.FromSearchResult(
                    _application.Root.SelectedSearchResult));

        public bool HasSelectedSearchResult =>
            _application.Root.SearchTerm != null &&
            _application.Root.SearchTerm.SearchResults.Any() &&
            _application.Root.SelectedSearchResult != null;

        public bool CanSubscribe =>
            _application.Root.SelectedSearchResult != null &&
            !_subscription.IsSubscribed(_application.Root.SelectedSearchResult.FeedUrl);

        public bool CanUnsubscribe =>
            _application.Root.SelectedSearchResult != null &&
            _subscription.IsSubscribed(_application.Root.SelectedSearchResult.FeedUrl);

        public void Subscribe()
        {
            if (_application.Root.SelectedSearchResult != null)
            {
                _subscription.Subscribe(_application.Root.SelectedSearchResult.FeedUrl);
                _subscription.ManagingSubscriptions = true;
            }
        }

        public void Unsubscribe()
        {
            if (_application.Root.SelectedSearchResult != null)
            {
                _subscription.Unsubscribe(_application.Root.SelectedSearchResult.FeedUrl);
            }
        }

        public string Message => ResourceLoader.GetForViewIndependentUse().GetString(
            (_application.Root.SearchTerm == null ||
             _application.Root.SearchTerm.IsBusy)
                ? "SearchBusy" :
            _application.Root.SelectedSearchResult == null
                ? "SearchResults"
                : "SearchDetails");

        public string LastException =>
            _application.Exception?.Message;
    }
}
