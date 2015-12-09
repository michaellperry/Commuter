using Assisticant;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter
{
    class ViewModelLocator : ViewModelLocatorBase
    {
        private Observable<Model> _model = new Observable<Model>(new Model());

        public Model Model
        {
            get { return _model.Value; }
        }

        public object OnboardingViewModel
        {
            get { return ViewModel(NewOnboardingViewModel); }
        }

        public object SearchViewModel
        {
            get { return ViewModel(NewSearchViewModel); }
        }

        private Onboarding.OnboardingViewModel NewOnboardingViewModel()
        {
            return new Onboarding.OnboardingViewModel(Model.SearchService);
        }

        private Search.SearchViewModel NewSearchViewModel()
        {
            return new Search.SearchViewModel(Model.SearchService, NewSearchResultViewModel);
        }

        private Search.SearchResultViewModel NewSearchResultViewModel(
            Search.SearchResult searchResult)
        {
            return new Search.SearchResultViewModel(searchResult);
        }
    }
}
