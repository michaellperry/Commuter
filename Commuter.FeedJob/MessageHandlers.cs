using Commuter.FeedJob.Entities;
using RoverMob.Messaging;
using System;
using System.IO;
using System.Linq;

namespace Commuter.FeedJob
{
    static class MessageHandlers
    {
        public static void HandleSubscribe(Message message, TextWriter log)
        {
            string feedUrl = message.Body.FeedUrl;
            string imageUri = message.Body.ImageUri;
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
                        FeedUrl = feedUrl,
                        ImageUri = imageUri
                    });
                    log.WriteLine($"Adding new podcast {feedUrl}.");
                }
                else
                {
                    log.WriteLine($"Adding subscription for existing podcast {feedUrl}.");
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

        public static void HandleUnsubscribe(Message message, TextWriter log)
        {
            var hashes = message.GetPredecessors("Subscription")
                .Select(p => Convert.ToBase64String(p.Code))
                .ToList();

            using (var context = new CommuterDbContext())
            {
                var predecessors = context.Subscriptions
                    .Where(s => hashes.Contains(s.Hash))
                    .ToList();
                context.Subscriptions.RemoveRange(predecessors);
                log.WriteLine($"Removing {predecessors.Count} podcast subscriptions");

                context.SaveChanges();
            }
        }
    }
}
