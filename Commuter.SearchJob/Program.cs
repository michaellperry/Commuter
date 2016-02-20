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
            var config = new JobHostConfiguration();
            var serviceBusConfig = new ServiceBusConfiguration()
            {
                ConnectionString = AmbientConnectionStringProvider
                    .Instance
                    .GetConnectionString(ConnectionStringNames.ServiceBus)
            };
            config.UseServiceBus(serviceBusConfig);
            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
