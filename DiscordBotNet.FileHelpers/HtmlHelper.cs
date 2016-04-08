using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotNet.FileHelpers
{
    public static class HtmlHelper
    {
        public static string JavascriptSearch(string html, string property, Type type)
        {
            var startIndex = html.IndexOf($"var {property} = ") + $"var {property} = ".Length;
            var endIndex = html.IndexOf(";", startIndex);
            var value = html.Substring(startIndex, endIndex - startIndex);
            if (type == typeof(string))
            {
                value = value.Substring(1, value.Length - 2);
                return value;
            }
            throw new NotImplementedException();
        }

        public static string XmlSearch(string xml, string property)
        {

            var startIndex = xml.IndexOf($"{property}=\"") + $"{property}=\"".Length;
            var endIndex = xml.IndexOf("\"", startIndex);
            var value = xml.Substring(startIndex, endIndex - startIndex);
            
            value = value.Substring(1, value.Length - 1);
            return value;
        }

        public static string XmlFindElement(string xml, string attribute, string value)
        {
            var searchString = $"{attribute}=\"{value}\"";
            var startIndex = xml.IndexOf(searchString) + searchString.Length;
            startIndex = PreviousIndex(xml, "<", startIndex);
            var endIndex = xml.IndexOf(">", startIndex) + ">".Length;

            var element = xml.Substring(startIndex, endIndex - startIndex);
            return element;
        }

        public static int PreviousIndex(string str, string search, int beforeIndex)
        {
            str = str.Substring(0, beforeIndex);
            return str.LastIndexOf("<");
        }
    }
}
