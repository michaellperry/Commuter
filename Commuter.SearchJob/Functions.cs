using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using RoverMob.Protocol;
using RoverMob.Messaging;
using System.Configuration;
using System.Collections.Immutable;
using Commuter.DigitalPodcast;
using RoverMob;

namespace Commuter.SearchJob
{
    public class Functions
    {
        public static void ProcessQueueMessage(
            [ServiceBusTrigger("commutermessages")] MessageMemento messageMemento,
            TextWriter log)
        {
            log.WriteLine($"Received message of type {messageMemento.MessageType}");
        }

        private static async Task<ImmutableList<SearchResult>> PerformSearch(
            string searchTerm)
        {
            var results = new SearchResult[0];

            return results.ToImmutableList();
        }

        private static Message CreateSearchResultMessage(
            MessageHash searchHash,
            Guid searchTermId,
            SearchResult searchResult)
        {
            var searchResultMessage = Message.CreateMessage(
                searchTermId.ToCanonicalString(),
                "SearchResult",
                Predecessors.Set
                    .In("Search", searchHash),
                searchTermId,
                new
                {
                    ProviderId = "digitalpodcast.com",
                    FeedUrl = searchResult.FeedUrl,
                    Title = searchResult.Title,
                    Subtitle = searchResult.Subtitle,
                    Author = searchResult.Author,
                    ImageUri = searchResult.ImageUri
                });
            return searchResultMessage;
        }

        private static Message CreateAggregateMessage(
            Guid searchTermId,
            ImmutableList<Message> searchResultMessages)
        {
            var aggregateMessage = Message.CreateMessage(
                searchTermId.ToCanonicalString(),
                "Aggregate",
                Predecessors.Set
                    .In("SearchResult", searchResultMessages.Select(r => r.Hash)),
                searchTermId,
                new { });
            return aggregateMessage;
        }

        private static HttpMessagePump GetMessagePump()
        {
            Uri distributorUrl = new Uri(
                ConfigurationManager.AppSettings["DistributorUrl"],
                UriKind.Absolute);
            var messageQueue = new NoOpMessageQueue();
            var bookmarkStore = new NoOpBookmarkStore();
            var pump = new HttpMessagePump(
                distributorUrl,
                messageQueue,
                bookmarkStore);
            return pump;
        }
    }
}
