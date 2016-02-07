using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Immutable;

namespace Commuter.DigitalPodcast
{
    public static class Opml
    {
        public static DigitalPodcastResponse Parse(XDocument document)
        {
            XElement element = document.Root;
            VerifyName(element, "opml");

            var body = VerifyElement(document.Root, "body");
            var results = body.Elements()
                .Select(e => ParseOutline(e))
                .ToImmutableList();

            return new DigitalPodcastResponse
            {
                Results = results
            };
        }

        private static DigitalPodcastResult ParseOutline(XElement element)
        {
            VerifyName(element, "outline");
            return new DigitalPodcastResult
            {
                Title = VerifyAttribute(element, "text").Value,
                FeedUrl = VerifyUrl(VerifyAttribute(element, "url").Value)
            };
        }

        private static void VerifyName(XElement element, string expected)
        {
            if (element.Name.LocalName != expected)
                throw new ArgumentException(string.Format("Expected {0}, but found {1}.", expected, element.Name.LocalName));
        }

        private static XElement VerifyElement(XElement parent, string name)
        {
            var element = parent.Element(name);
            if (element == null)
                throw new ArgumentException(string.Format("Expected to find an element named {0}, but found none.", name));
            return element;
        }

        private static XAttribute VerifyAttribute(XElement element, string name)
        {
            XAttribute attribute = element.Attribute(name);
            if (attribute == null)
                throw new ArgumentException(string.Format("Expected to find an attribute named {0}, but found none", name));
            return attribute;
        }

        private static Uri VerifyUrl(string value)
        {
            Uri result;
            if (!Uri.TryCreate(value, UriKind.Absolute, out result))
                throw new ArgumentException(string.Format("Expected to find a URL, but found {0}.", value));
            return result;
        }
    }
}
