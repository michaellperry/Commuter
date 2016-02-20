using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Azure.WebJobs.Host;

namespace Commuter.SearchJob
{
    public class Program
    {
        public static void Main()
        {
            var host = new JobHost();
            host.RunAndBlock();
        }
    }
}
