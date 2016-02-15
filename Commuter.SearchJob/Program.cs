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
            string connectionString = AmbientConnectionStringProvider.Instance
                .GetConnectionString(ConnectionStringNames.ServiceBus);
            var serviceBusConfig = new ServiceBusConfiguration()
            {
                ConnectionString = connectionString
            };
            config.UseServiceBus(serviceBusConfig);
            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
