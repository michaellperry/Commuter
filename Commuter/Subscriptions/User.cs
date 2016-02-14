using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commuter.Subscriptions
{
    public class User : IMessageHandler
    {
        public IEnumerable<IMessageHandler> Children
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Guid GetObjectId()
        {
            throw new NotImplementedException();
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            throw new NotImplementedException();
        }

        public void HandleMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
