using System;
using DiscordBotNet.FileHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiscordBotNet.Tests.FileHelpers
{
    [TestClass]
    public class HtmlHelperTests
    {
        [TestMethod]
        public void XmlFindElementTest()
        {
            var element = HtmlHelper.XmlFindElement("<div><div id=\"test\"></div></div>", "id", "test");

            Assert.AreEqual("<div id=\"test\">", element);
        }
    }
}
