using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Commuter.DigitalPodcast.Opml
{
    [DataContract]
    public class Head
    {
        [DataMember, XmlText]
        public string Title { get; set; }
    }
}
