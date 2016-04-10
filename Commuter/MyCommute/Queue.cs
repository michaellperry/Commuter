using Assisticant.Collections;
using Assisticant.Fields;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Commuter.MyCommute
{
    public class Queue : IMessageHandler
    {
        private MessageDispatcher<Queue> _dispatcher = new MessageDispatcher<Queue>()
            .On("Downloaded", (q, m) => q.OnDownloaded(m))
            .On("Playhead", (q, m) => q.OnPlayhead(m));

        public Guid ObjectId { get; }
        public Uri MediaUrl { get; }
        public string Title { get; }
        public string Summary { get; }
        public DateTime PublishedDate { get; }
        public Uri ImageUri { get; }

        private readonly Guid _userGuid;
        private Observable<string> _fileName = new Observable<string>();

        private ObservableList<TimeSpan> _playheads =
            new ObservableList<TimeSpan>();

        public Queue(
            Guid objectId,
            Guid userGuid,
            Uri mediaUrl,
            string title,
            string summary,
            DateTime publishedDate,
            Uri imageUri)
        {
            ObjectId = objectId;
            _userGuid = userGuid;
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
        public TimeSpan Position => _playheads
            .DefaultIfEmpty(TimeSpan.Zero)
            .Max();

        public void OnDownloaded(Message message)
        {
            _fileName.Value = message.Body.FileName;
        }

        public Message SetPlayhead(TimeSpan position)
        {
            return Message.CreateMessage(
                _userGuid.ToCanonicalString(),
                "Playhead",
                ObjectId,
                new
                {
                    Position = position.TotalSeconds
                });
        }

        private void OnPlayhead(Message message)
        {
            double position = message.Body.Position;
            _playheads.Add(TimeSpan.FromSeconds(position));
        }
    }
}
