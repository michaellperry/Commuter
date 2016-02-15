using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using RoverMob.Protocol;

namespace Commuter.SearchJob
{
    public class Functions
    {
        public static void ProcessQueueMessage(
            [ServiceBusTrigger("commutermessages")] MessageMemento message,
            TextWriter log)
        {
            log.WriteLine($"Received message of type {message.MessageType}");
        }
    }
}
