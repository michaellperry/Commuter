using Commuter.FeedJob.Entities;
using Commuter.FeedJob.Migrations;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using System.Data.Entity;

namespace Commuter.FeedJob
{
    class Program
    {
        static void Main()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<CommuterDbContext, Configuration>());

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
