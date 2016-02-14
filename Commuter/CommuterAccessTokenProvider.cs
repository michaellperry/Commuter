using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter
{
    public class CommuterAccessTokenProvider : IAccessTokenProvider
    {
        public CommuterAccessTokenProvider()
        {
            
        }

        public Task<string> GetAccessTokenAsync()
        {
            throw new NotImplementedException();
        }

        public void RefreshAccessToken()
        {
            throw new NotImplementedException();
        }
    }
}
