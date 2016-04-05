using Assisticant.Fields;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Commuter.MyCommute
{
    public class Queue : IMessageHandler
    {
        private MessageDispatcher<Queue> _dispatcher = new MessageDispatcher<Queue>()
            .On("Downloaded", (q, m) => q.OnDownloaded(m));

        public Guid ObjectId { get; }
        public Uri MediaUrl { get; }
        public string Title { get; }
        public string Summary { get; }
        public DateTime PublishedDate { get; }
        public Uri ImageUri { get; }

        private Observable<string> _fileName = new Observable<string>();

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
            foreach (var message in messages)
                _dispatcher.Dispatch(this, message);
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        public bool IsDownloaded => !string.IsNullOrEmpty(_fileName.Value);
        public string FileName => _fileName.Value;

        public void OnDownloaded(Message message)
        {
            _fileName.Value = message.Body.FileName;
        }
    }
}
