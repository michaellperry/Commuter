using System;
using RoverMob.Messaging;

namespace Commuter.Details
{
    public class Episode
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime PublishDate { get; set; }
        public Uri MediaUrl { get; set; }
        public MessageHash Hash { get; set; }
    }
}
