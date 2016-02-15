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

namespace Commuter.SearchJob
{
    public class Functions
    {
        public static void ProcessQueueMessage(
            [ServiceBusTrigger("commutermessages")] MessageMemento messageMemento,
            TextWriter log)
        {
            log.WriteLine($"Received message of type {messageMemento.MessageType}");

            if (messageMemento.MessageType == "Search")
                HandleSearch(Message.FromMemento(messageMemento), log).Wait();
        }

        private static async Task HandleSearch(
            Message searchMessage,
            TextWriter log)
        {
            HttpMessagePump pump = GetMessagePump();

            var searchTermHash = searchMessage
                .GetPredecessors("SearchTerm");
            string topic = searchMessage.Body.SearchTermId;
            string searchTerm = searchMessage.Body.SearchTerm;

            log.WriteLine($"Searching for {searchTerm}");
            var searchResults = await PerformSearch(searchTerm);
            log.WriteLine($"Found {searchResults.Count} results");

            var searchResultMessages = searchResults
                .Select(r => CreateSearchResultMessage(
                    searchTermHash, topic, r))
                .ToImmutableList();
            pump.SendAllMessages(searchResultMessages);

            var aggregateMessage = Message.CreateMessage(
                topic,
                "Aggregate",
                Predecessors.Set
                    .In("SearchResult", searchResultMessages.Select(r => r.Hash))
                    .In("Search", searchMessage.Hash),
                Guid.NewGuid(),
                new { });
            pump.Enqueue(aggregateMessage);
        }

        private static async Task<ImmutableList<SearchResult>> PerformSearch(
            string searchTerm)
        {
            var search = new DigitalPodcastSearch(
                ConfigurationManager.AppSettings["DigitalPodcastApiKey"]);
            var response = await search.SearchAsync(
                new DigitalPodcastRequest
                {
                    Keywords = searchTerm
                });
            var tasks = response.Results
                .Select(r => SearchResult.TryLoadAsync(r.FeedUrl));
            var allResults = await Task.WhenAll(tasks);
            var results = allResults
                .Where(r => r != null);

            return results.ToImmutableList();
        }

        private static Message CreateSearchResultMessage(
            ImmutableList<MessageHash> searchTermHash,
            string topic,
            SearchResult searchResult)
        {
            var searchResultMessage = Message.CreateMessage(
                topic,
                "SearchResult",
                Predecessors.Set
                    .In("SearchTerm", searchTermHash),
                Guid.NewGuid(),
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
