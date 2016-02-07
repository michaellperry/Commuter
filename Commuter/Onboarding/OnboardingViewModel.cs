using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Onboarding
{
    class OnboardingViewModel
    {
        private Search.SearchService _search;

        public OnboardingViewModel(Search.SearchService search)
        {
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

        public string LastException
        {
            get { return _search.Exception?.Message; }
        }
    }
}
