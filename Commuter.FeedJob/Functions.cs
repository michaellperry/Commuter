using Microsoft.Azure.WebJobs;
using RoverMob.Messaging;
using RoverMob.Protocol;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Commuter.FeedJob
{
    public class Functions
    {
        public static void ProcessQueueMessage(
            [ServiceBusTrigger("subscriptionmessages")] MessageMemento messageMemento,
            TextWriter log)
        {
            log.WriteLine($"Received message of type {messageMemento.MessageType}");

            if (messageMemento.MessageType == "Subscribe")
                HandleSubscribe(Message.FromMemento(messageMemento), log).Wait();
            if (messageMemento.MessageType == "Unsubscribe")
                HandleUnsubscribe(Message.FromMemento(messageMemento), log).Wait();
        }

        private static async Task HandleSubscribe(Message message, TextWriter log)
        {
            throw new NotImplementedException();
        }

        private static async Task HandleUnsubscribe(Message message, TextWriter log)
        {
            throw new NotImplementedException();
        }
    }
}
