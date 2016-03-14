using System.Linq;
using System.Text.RegularExpressions;

namespace Commuter.PodcastFeed
{
    public class HtmlParser
    {
        private static Regex HtmlElement = new Regex("<[a-zA-Z/]*>");

        public static string ContentOfHtml(string html)
        {
            return string.Join(" ", HtmlElement
                .Split(html)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray());
        }
    }
}
