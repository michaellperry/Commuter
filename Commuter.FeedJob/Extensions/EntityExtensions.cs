using Commuter.FeedJob.Entities;
using RoverMob;
using System;

namespace Commuter.FeedJob
{
    static class EntityExtensions
    {
        public static Guid ToGuid(this Podcast podcast)
        {
            return new { FeedUrl = podcast.FeedUrl }.ToGuid();
        }
    }
}
