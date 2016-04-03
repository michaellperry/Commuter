using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Commuter.MyCommute
{
    public class Queue : IMessageHandler
    {
        public Guid ObjectId { get; }
        public Uri MediaUrl { get; }
        public string Title { get; }
        public string Summary { get; }
        public DateTime PublishedDate { get; }
        public Uri ImageUri { get; }

        public Queue(
            Guid objectId,
            Uri mediaUrl,
            string title,
            string summary,
            DateTime publishedDate,
            Uri imageUri)
        {
            ObjectId = objectId;
            MediaUrl = mediaUrl;
            Title = title;
            Summary = summary;
            PublishedDate = publishedDate;
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
