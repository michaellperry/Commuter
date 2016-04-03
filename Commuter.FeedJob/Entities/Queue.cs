namespace Commuter.FeedJob.Entities
{
    public class Queue
    {
        public int QueueId { get; set; }

        public int SubscriptionId { get; set; }
        public virtual Subscription Subscription { get; set; }

        public int EpisodeId { get; set; }
        public virtual Episode Episode { get; set; }
    }
}
