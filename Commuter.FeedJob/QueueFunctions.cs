using Commuter.FeedJob.Entities;
using RoverMob;
using RoverMob.Messaging;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Commuter.FeedJob
{
    static class QueueFunctions
    {
        public static async Task CheckQueues(TextWriter log, HttpMessagePump pump)
        {
            using (var context = new CommuterDbContext())
            {
                var episodesToQueue = (
                    from s in context.Subscriptions
                    where s.StartAtEpisodeId > 0
                    from e in s.Podcast.Episodes
                    where e.EpisodeId >= s.StartAtEpisodeId
                    where !(
                        from q in s.Queues
                        where q.Episode == e
                        select q
                    ).Any()
                    select new { Subscription = s, Episode = e })
                    .ToImmutableList();

                var distinctEpisodes = episodesToQueue
                    .Select(e => e.Episode)
                    .Distinct()
                    .Count();
                var distinctSubscriptions = episodesToQueue
                    .Select(e => e.Subscription)
                    .Distinct()
                    .Count();
                log.WriteLine($"Found {distinctEpisodes} new episodes to queue for {distinctSubscriptions} subscriptions.");

                pump.SendAllMessages(episodesToQueue.Select(e =>
                    QueueMessage(e.Subscription, e.Episode))
                    .ToImmutableList());
                await pump.JoinAsync();
                if (pump.Exception == null)
                    throw pump.Exception;

                log.WriteLine($"Sent {episodesToQueue.Count} messages.");

                foreach (var e in episodesToQueue)
                {
                    e.Subscription.Queues.Add(new Queue
                    {
                        Episode = e.Episode
                    });
                }

                context.ValidateAndSaveChanges();

                log.WriteLine($"Saved {episodesToQueue.Count} records.");
            }
        }

        private static Message QueueMessage(Subscription subscription, Episode episode)
        {
            return Message.CreateMessage(
                subscription.UserGuid.ToCanonicalString(),
                "Queue",
                subscription.Podcast.ToGuid(),
                new
                {
                    Title = episode.Title,
                    Summary = episode.Summary,
                    PublishedDate = episode.PublishDate,
                    MediaUrl = episode.MediaUrl
                });
        }
    }
}
