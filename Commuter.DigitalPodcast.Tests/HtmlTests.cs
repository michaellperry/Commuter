using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commuter.DigitalPodcast.Tests
{
    [TestClass]
    public class HtmlTests
    {
        [TestMethod]
        public void CanExtractTextFromHtml()
        {
            string html = "<p>Paragraph one.</p><p>Paragraph <i>two</i>.</p><ul><li>First</li><li>Second</li><li>Third</li></ul><p>Paragraph <strong>three</strong>.</p>";
            string text = HtmlParser.ContentOfHtml(html);
            Assert.AreEqual("Paragraph one. Paragraph two . First Second Third Paragraph three .", text);
        }
    }
}
