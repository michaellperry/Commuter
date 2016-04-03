using Commuter.FeedJob.Entities;
using Commuter.PodcastFeed;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Commuter.FeedJob
{
    static class FeedFunctions
    {
        public static async Task CheckFeeds(TextWriter log, HttpMessagePump pump)
        {
            var yesterday = DateTime.UtcNow.AddDays(-1.0);
            var halfHourAgo = DateTime.UtcNow.AddHours(-0.5);

            using (var context = new CommuterDbContext())
            {
                var podcastsToCheck = context.Podcasts
                    .Include("Episodes")
                    .Where(p => context.Subscriptions.Any(
                        s => s.Podcast == p))
                    .Where(p =>
                        p.LastUpdateDateTime == null ||
                        p.LastUpdateDateTime < yesterday)
                    .Where(p =>
                        p.LastAttemptDateTime == null ||
                        p.LastAttemptDateTime < halfHourAgo)
                    .ToImmutableList();

                var messages = await Task.WhenAll(podcastsToCheck
                    .Select(p => CheckFeed(p, log)));

                pump.SendAllMessages(messages
                    .SelectMany(m => m)
                    .ToImmutableList());
                await pump.JoinAsync();
                if (pump.Exception == null)
                    throw pump.Exception;

                context.ValidateAndSaveChanges();
            }
        }

        private static async Task<ImmutableList<Message>> CheckFeed(
            Podcast podcast, TextWriter log)
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
                        Title = episode.Title.Truncate(50),
                        Summary = episode.Summary,
                        PublishDate = episode.PublishDate,
                        MediaUrl = episode.MediaUrl
                    });
                }
                podcast.LastUpdateDateTime = now;

                log.WriteLine($"Found {newEpisodes.Count} new episodes in {podcast.FeedUrl}.");

                Guid podcastGuid = podcast.ToGuid();
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
        
    }
}
