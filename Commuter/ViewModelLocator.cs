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
            get { return ViewModel(() => new Onboarding.OnboardingViewModel(Model)); }
        }
    }
}
