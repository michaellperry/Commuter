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
            return Task.FromResult(string.Empty);
        }

        public void RefreshAccessToken()
        {
        }
    }
}
