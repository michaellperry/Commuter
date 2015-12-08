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
        private readonly Model _model;
        private readonly Func<SearchResult, SearchResultViewModel> _newSearchResultViewModel;

        public SearchViewModel(
            Model model,
            Func<SearchResult, SearchResultViewModel> newSearchResultViewModel)
        {
            _model = model;
            _newSearchResultViewModel = newSearchResultViewModel;
        }

        public void GoBack()
        {
            if (_model.SelectedSearchResult != null)
                _model.SelectedSearchResult = null;
            else
                _model.ClearSearchResults();
        }

        public ImmutableList<SearchResultViewModel> SearchResults
        {
            get
            {
                return (
                    from searchResult in _model.SearchResults
                    orderby searchResult.Quality descending
                    select _newSearchResultViewModel(searchResult)
                    ).ToImmutableList();
            }
        }

        public SearchResultViewModel SelectedSearchResult
        {
            get
            {
                return _model.SelectedSearchResult == null
                    ? null
                    : _newSearchResultViewModel(_model.SelectedSearchResult);
            }
            set
            {
                _model.SelectedSearchResult = value == null
                    ? null
                    : value.SearchResult;
            }
        }

        public bool HasSelectedSearchResult
        {
            get { return _model.SelectedSearchResult != null; }
        }
    }
}
