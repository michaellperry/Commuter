using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/*
<?xml version="1.0" encoding="UTF-8"?>
<opml version="1.1" xmlns:podcastSearch="http://api.digitalpodcast.com/podcastsearchservice/output_specs.html">
  <head>
    <title>Digital Podcast Search - sharepoint</title>
    <dateCreated>Weds, 13 July 2005 13:55:28 GMT</dateCreated>
    <dateModified>Sun, 31 Jan 2016 19:53:33 -0500</dateModified>
    <ownerName>Alex Nesbitt</ownerName>
    <ownerEmail>digitalpodcast@gmail.com</ownerEmail>
    <podcastSearch:format>opml</podcastSearch:format>
    <podcastSearch:totalResults>8</podcastSearch:totalResults>
    <podcastSearch:startIndex>0</podcastSearch:startIndex>
    <podcastSearch:itemsPerPage>10</podcastSearch:itemsPerPage>
  </head>
  <body>
    <outline text="SharePoint 4 Every 1" type="link" url="http://feeds.feedburner.com/Sharepoint_4_Every_1"/>
    <outline text="SharePoint Pod Show" type="link" url="http://feeds.feedburner.com/SharepointPodShow"/>
    <outline text="The MOSS Show SharePoint Podcast" type="link" url="http://feeds.feedburner.com/TheMossShow"/>
    <outline text="Todd Klindt's SharePoint Admin Netcast" type="link" url="http://www.toddklindt.com/netcast/MP3/feed-mp3.rss"/>
    <outline text="Microsoft Cloud Show" type="link" url="http://feeds.microsoftcloudshow.com/MicrosoftCloudShowEpisodes"/>
    <outline text="OnMicrosoft (audio)" type="link" url="http://onmicrosoftaud.pearson.libsynpro.com/rss"/>
    <outline text="OnMicrosoft" type="link" url="http://onmicrosoftvid.pearson.libsynpro.com/rss"/>
    <outline text="TA Expert Interviews" type="link" url="http://technologyadvice.libsyn.com/rss"/>
  </body>
</opml>
*/

namespace Commuter.DigitalPodcast.Opml
{
    [DataContract(Name ="opml"), XmlSerializerFormat]
    class OpmlResponse
    {
        [DataMember(Name ="version"), XmlAttribute]
        public string Version { get; set; }

        [DataMember(Name ="head")]
        public Head Head { get; set; }
    }
}
