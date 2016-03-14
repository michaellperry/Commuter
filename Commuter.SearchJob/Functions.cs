using Commuter.DigitalPodcast;
using Microsoft.Azure.WebJobs;
using RoverMob;
using RoverMob.Messaging;
using RoverMob.Protocol;
using System;
using System.Collections.Immutable;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Commuter.SearchJob
{
    public class Functions
    {
        public static void ProcessQueueMessage(
            [ServiceBusTrigger("searchmessages")] MessageMemento messageMemento,
            TextWriter log)
        {
            log.WriteLine($"Received message of type {messageMemento.MessageType}");

            if (messageMemento.MessageType == "Search")
                HandleSearch(Message.FromMemento(messageMemento), log).Wait();
        }

        private static async Task HandleSearch(Message searchMessage, TextWriter log)
        {
            string searchTerm = searchMessage.Body.SearchTerm;

            log.WriteLine($"Searching for {searchTerm}");
            var searchResults = await PerformSearch(searchTerm);
            log.WriteLine($"Found {searchResults.Count} results");

            var searchTermId = new { Text = searchTerm }.ToGuid();
            var searchResultMessages = searchResults.Select(r => CreateSearchResultMessage(
                searchMessage.Hash,
                searchTermId,
                r)).ToImmutableList();

            var aggregateMessage = CreateAggregateMessage(searchTermId, searchResultMessages);

            var pump = GetMessagePump();
            pump.SendAllMessages(searchResultMessages);
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
