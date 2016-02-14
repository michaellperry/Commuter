using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Create a partial class in Secrets.Private.cs:

namespace Commuter
{
    partial class Secrets
    {
        partial void Initialize()
        {
            DigitalPodcastApiKey = "Register for your own API key";
        }
    }
}
*/

namespace Commuter
{
    partial class Secrets
    {
        public string DigitalPodcastApiKey { get; set; }

        public Secrets()
        {
            Initialize();
        }

        partial void Initialize();
    }
}
