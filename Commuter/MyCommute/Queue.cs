using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Commuter.MyCommute
{
    class Queue : IMessageHandler
    {
        public Guid ObjectId { get; }
        public Uri MediaUrl { get; }
        public string Title { get; }
        public string Summary { get; }
        public DateTime PublishedDated { get; }
        public Uri ImageUri { get; }

        public Queue(
            Guid objectId,
            Uri mediaUrl,
            string title,
            string summary,
            DateTime publishedDated,
            Uri imageUri)
        {
            ObjectId = objectId;
            MediaUrl = mediaUrl;
            Title = title;
            Summary = summary;
            PublishedDated = publishedDated;
            ImageUri = imageUri;
        }

        public IEnumerable<IMessageHandler> Children =>
            ImmutableList<IMessageHandler>.Empty;

        public Guid GetObjectId()
        {
            return ObjectId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
        }

        public void HandleMessage(Message message)
        {
        }
    }
}
