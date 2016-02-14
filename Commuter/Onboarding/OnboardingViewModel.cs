using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Onboarding
{
    class OnboardingViewModel
    {
        private readonly CommuterApplication _application;
        private readonly Search.SearchService _search;

        public OnboardingViewModel(
            CommuterApplication application,
            Search.SearchService search)
        {
            _application = application;
            _search = search;
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

        public string LastException =>
            _application.Exception?.Message ??
            _search.Exception?.Message;
    }
}
