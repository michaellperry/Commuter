using Microsoft.Azure.WebJobs;
using RoverMob.Messaging;
using RoverMob.Protocol;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Commuter.FeedJob
{
    public class Functions
    {
        public static void ProcessQueueMessage(
            [ServiceBusTrigger("subscriptionmessages")] MessageMemento messageMemento,
            TextWriter log)
        {
            log.WriteLine($"Received message of type {messageMemento.MessageType}");

            if (messageMemento.MessageType == "Subscribe")
                HandleSubscribe(Message.FromMemento(messageMemento), log);
            if (messageMemento.MessageType == "Unsubscribe")
                HandleUnsubscribe(Message.FromMemento(messageMemento), log);
        }

        public static void CheckPodcastFeedsOnTimer(
            [TimerTrigger("0 */5 * * * *")] TimerInfo timer,
            TextWriter log)
        {
            CheckFeeds(log).Wait();
        }

        private static void HandleSubscribe(Message message, TextWriter log)
        {
            MessageHandlers.HandleSubscribe(message, log);

            CheckFeeds(log).Wait();
        }

        private static void HandleUnsubscribe(Message message, TextWriter log)
        {
            MessageHandlers.HandleUnsubscribe(message, log);

            CheckFeeds(log).Wait();
        }

        private static async Task CheckFeeds(TextWriter log)
        {
            var pump = GetMessagePump();

            await FeedFunctions.CheckFeeds(log, pump);
            await QueueFunctions.CheckQueues(log, pump);
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
