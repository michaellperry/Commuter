using Commuter.FeedJob.Entities;
using Commuter.PodcastFeed;
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

            CheckFeeds(log).Wait();
        }

        private static void HandleUnsubscribe(Message message, TextWriter log)
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

            CheckFeeds(log).Wait();
        }

        private static async Task CheckFeeds(TextWriter log)
        {
            var yesterday = DateTime.UtcNow.AddDays(-1.0);
            var halfHourAgo = DateTime.UtcNow.AddHours(-0.5);

            using (var context = new CommuterDbContext())
            {
                var podcastsToCheck = context.Podcasts
                    .Include("Episodes")
                    .Where(p => context.Subscriptions.Any(s => s.Podcast == p))
                    .Where(p =>
                        p.LastUpdateDateTime == null ||
                        p.LastUpdateDateTime < yesterday)
                    .Where(p =>
                        p.LastAttemptDateTime == null ||
                        p.LastAttemptDateTime < halfHourAgo)
                    .Distinct()
                    .ToImmutableList();

                var messages = await Task.WhenAll(podcastsToCheck
                    .Select(p => CheckFeed(p, log)));

                var pump = GetMessagePump();
                pump.SendAllMessages(messages
                    .SelectMany(m => m)
                    .ToImmutableList());

                context.SaveChanges();
            }
        }

        private static async Task<ImmutableList<Message>> CheckFeed(Podcast podcast, TextWriter log)
        {
            var now = DateTime.UtcNow;
            try
            {
                podcast.LastAttemptDateTime = now;
                var result = await PodcastFunctions.TryLoadAsync(
                    new Uri(podcast.FeedUrl, UriKind.Absolute));

                var newEpisodes = result.Episodes
                    .Where(e1 => !podcast.Episodes
                        .Any(e2 => e1.MediaUrl == e2.MediaUrl))
                    .ToImmutableList();
                foreach (var episode in newEpisodes)
                {
                    podcast.Episodes.Add(new Entities.Episode
                    {
                        Title = episode.Title,
                        Summary = episode.Summary,
                        PublishDate = episode.PublishDate,
                        MediaUrl = episode.MediaUrl
                    });
                    log.WriteLine($"Found podcast episode \"{episode.Title}\".");
                }
                podcast.LastUpdateDateTime = now;

                Guid podcastGuid = new { FeedUrl = podcast.FeedUrl }.ToGuid();
                return newEpisodes
                    .Select(episode => Message.CreateMessage(
                        podcastGuid.ToCanonicalString(),
                        "Episode",
                        podcastGuid,
                        new
                        {
                            Title = episode.Title,
                            Summary = episode.Summary,
                            PublishDate = episode.PublishDate,
                            MediaUrl = episode.MediaUrl
                        }))
                    .ToImmutableList();
            }
            catch (Exception)
            {
                // TODO: Publish the error so that someone can verify the podcast URL.
                return ImmutableList<Message>.Empty;
            }
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
