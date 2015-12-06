using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Onboarding
{
    class OnboardingViewModel
    {
        private Model _model;

        public OnboardingViewModel(Model model)
        {
            _model = model;
        }

        public string SearchTerm
        {
            get { return _model.SearchTerm; }
            set { _model.SearchTerm = value; }
        }

        public void QuerySubmitted()
        {
            _model.BeginSearch();
        }
    }
}
