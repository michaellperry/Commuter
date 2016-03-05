using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Onboarding
{
    class LoginViewModel
    {
        private readonly CommuterApplication _application;

        public LoginViewModel(
            CommuterApplication application)
        {
            _application = application;
        }

        public bool HasException => _application.Exception != null;
        public string LastException => _application.Exception?.Message;
    }
}
