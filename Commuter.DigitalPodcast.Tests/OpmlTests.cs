using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Commuter.DigitalPodcast.Tests
{
    [TestClass]
    public class OpmlTests
    {
        [TestMethod]
        public void CanReadOpml()
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output);
            writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            writer.WriteLine(@"<opml version=""1.1"" xmlns:podcastSearch=""http://api.digitalpodcast.com/podcastsearchservice/output_specs.html"">");
            writer.WriteLine(@"  <head>");
            writer.WriteLine(@"    <title>Digital Podcast Search - sharepoint</title>");
            writer.WriteLine(@"    <dateCreated>Weds, 13 July 2005 13:55:28 GMT</dateCreated>");
            writer.WriteLine(@"    <dateModified>Sun, 31 Jan 2016 19:53:33 -0500</dateModified>");
            writer.WriteLine(@"    <ownerName>Alex Nesbitt</ownerName>");
            writer.WriteLine(@"    <ownerEmail>digitalpodcast@gmail.com</ownerEmail>");
            writer.WriteLine(@"    <podcastSearch:format>opml</podcastSearch:format>");
            writer.WriteLine(@"    <podcastSearch:totalResults>8</podcastSearch:totalResults>");
            writer.WriteLine(@"    <podcastSearch:startIndex>0</podcastSearch:startIndex>");
            writer.WriteLine(@"    <podcastSearch:itemsPerPage>10</podcastSearch:itemsPerPage>");
            writer.WriteLine(@"  </head>");
            writer.WriteLine(@"  <body>");
            writer.WriteLine(@"    <outline text=""SharePoint 4 Every 1"" type=""link"" url=""http://feeds.feedburner.com/Sharepoint_4_Every_1""/>");
            writer.WriteLine(@"    <outline text=""SharePoint Pod Show"" type=""link"" url=""http://feeds.feedburner.com/SharepointPodShow""/>");
            writer.WriteLine(@"    <outline text=""The MOSS Show SharePoint Podcast"" type=""link"" url=""http://feeds.feedburner.com/TheMossShow""/>");
            writer.WriteLine(@"    <outline text=""Todd Klindt's SharePoint Admin Netcast"" type=""link"" url=""http://www.toddklindt.com/netcast/MP3/feed-mp3.rss""/>");
            writer.WriteLine(@"    <outline text=""Microsoft Cloud Show"" type=""link"" url=""http://feeds.microsoftcloudshow.com/MicrosoftCloudShowEpisodes""/>");
            writer.WriteLine(@"    <outline text=""OnMicrosoft (audio)"" type=""link"" url=""http://onmicrosoftaud.pearson.libsynpro.com/rss""/>");
            writer.WriteLine(@"    <outline text=""OnMicrosoft"" type=""link"" url=""http://onmicrosoftvid.pearson.libsynpro.com/rss""/>");
            writer.WriteLine(@"    <outline text=""TA Expert Interviews"" type=""link"" url=""http://technologyadvice.libsyn.com/rss""/>");
            writer.WriteLine(@"  </body>");
            writer.WriteLine(@"</opml>");
            writer.Flush();
            var bytes = output.ToArray();

            var input = new MemoryStream(bytes);

            var document = XDocument.Load(input);
            Assert.AreEqual("opml", document.Root.Name);
        }
    }
}
