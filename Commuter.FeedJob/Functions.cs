using Commuter.FeedJob.Entities;
using Microsoft.Azure.WebJobs;
using RoverMob.Messaging;
using RoverMob.Protocol;
using System;
using System.IO;
using System.Linq;
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

        private static void HandleSubscribe(Message message, TextWriter log)
        {
            string feedUrl = message.Body.FeedUrl;
            string hash = Convert.ToBase64String(message.Hash.Code);
            Guid userGuid = message.ObjectId;

            using (var context = new CommuterDbContext())
            {
                Podcast podcast = context.Podcasts
                    .FirstOrDefault(p => p.FeedUrl == feedUrl);
                if (podcast == null)
                {
                    podcast = context.Podcasts.Add(new Podcast
                    {
                        FeedUrl = feedUrl
                    });
                }

                context.Subscriptions.Add(new Subscription
                {
                    Podcast = podcast,
                    UserGuid = userGuid,
                    Hash = hash
                });

                context.SaveChanges();
            }
        }

        private static void HandleUnsubscribe(Message message, TextWriter log)
        {
            var hashes = message.GetPredecessors("Subscription")
                .Select(p => Convert.ToBase64String(p.Code))
                .ToList();

            using (var context = new CommuterDbContext())
            {
                var predecessors = context.Subscriptions
                    .Where(s => hashes.Contains(s.Hash));
                context.Subscriptions.RemoveRange(predecessors);

                context.SaveChanges();
            }
        }
    }
}
