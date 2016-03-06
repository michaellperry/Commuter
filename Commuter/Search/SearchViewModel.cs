using Commuter.Details;
using Commuter.Subscriptions;
using System;
using System.Collections.Immutable;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace Commuter.Search
{
    class SearchViewModel
    {
        private readonly SearchService _search;
        private readonly PodcastService _podcastService;
        private readonly SubscriptionService _subscription;
        private readonly Func<SearchResult, SearchResultViewModel> _newSearchResultViewModel;
        private readonly Func<Podcast, DetailViewModel> _newDetailViewModel;

        public SearchViewModel(
            SearchService search,
            PodcastService podcastService,
            SubscriptionService subscription,
            Func<SearchResult, SearchResultViewModel> newSearchResultViewModel,
            Func<Podcast, DetailViewModel> newDetailViewModel)
        {
            _search = search;
            _podcastService = podcastService;
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

        public string Heading => String.IsNullOrEmpty(_search.ActiveSearchTerm?.Text)
            ? null
            : $"'{_search.ActiveSearchTerm.Text}' Results";

        public void GoBack()
        {
            if (_search.SelectedSearchResult != null)
                _search.SelectedSearchResult = null;
            else
                _search.ClearSearch();
        }

        public ImmutableList<SearchResultViewModel> SearchResults =>
            _search.ActiveSearchTerm == null
                ? ImmutableList<SearchResultViewModel>.Empty
                :   (
                    from searchResult in _search.ActiveSearchTerm.SearchResults
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

        public DetailViewModel SelectedPodcastDetail =>
            _search.SelectedSearchResult == null
                ? null
                : _newDetailViewModel(CreatePodcast(
                    _search.SelectedSearchResult));

        public bool HasSelectedSearchResult =>
            _search.ActiveSearchTerm != null &&
            _search.ActiveSearchTerm.SearchResults.Any() &&
            _search.SelectedSearchResult != null;

        public bool CanSubscribe =>
            _search.SelectedSearchResult != null &&
            !_subscription.IsSubscribed(_search.SelectedSearchResult.FeedUrl);

        public bool CanUnsubscribe =>
            _search.SelectedSearchResult != null &&
            _subscription.IsSubscribed(_search.SelectedSearchResult.FeedUrl);

        public void Subscribe()
        {
            SearchResult searchResult = _search.SelectedSearchResult;
            if (searchResult != null)
            {
                _subscription.Subscribe(
                    searchResult.FeedUrl,
                    searchResult.ImageUri,
                    searchResult.Title,
                    searchResult.Author);
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
            (_search.ActiveSearchTerm == null ||
             _search.ActiveSearchTerm.IsBusy)
                ? "SearchBusy" :
            _search.SelectedSearchResult == null
                ? "SearchResults"
                : "SearchDetails");

        public bool Busy =>
            (_search.ActiveSearchTerm != null &&
             _search.ActiveSearchTerm.IsBusy) ||
            _podcastService.Busy;

        public string LastException => null;

        private Podcast CreatePodcast(SearchResult searchResult)
        {
            var podcast = new Podcast
            {
                Title = searchResult.Title,
                Subtitle = searchResult.Subtitle,
                Author = searchResult.Author,
                FeedUrl = searchResult.FeedUrl,
                ImageUri = searchResult.ImageUri
            };
            _podcastService.BeginLoadPodcast(podcast);
            return podcast;
        }
    }
}
