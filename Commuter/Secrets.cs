/*
Create a partial class in Secrets.Private.cs:

namespace Commuter
{
    partial class Secrets
    {
        partial void Initialize()
        {
            DistributorUrl = "http://<Deploy a web app>/api/distributor/";
            UserIdentifierUrl = "http://<Deploy a web app>/api/useridentifier/";
            NotificationHubPath = "Create an Azure Notification Hub";
            NotificationHubConnectionString = "The connection string with Listen privileges";
        }
    }
}
*/

namespace Commuter
{
    partial class Secrets
    {
        public string DistributorUrl { get; set; }
        public string UserIdentifierUrl { get; set; }
        public string NotificationHubPath { get; set; }
        public string NotificationHubConnectionString { get; set; }

        public Secrets()
        {
            Initialize();
        }

        partial void Initialize();
    }
}
