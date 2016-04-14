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

        public void GetElementIndex()
        {
            var str = @"<html>
  <head>
    <title>905: Bedroom</title>
    <style type=""text / css""> <!--
        .status {
                position: fixed; top: 1px; color: #f0f0f0; background: #0f0f0f; width: 100% } 
    --> </ style >
  </ head >
< body OnLoad = ""document.f.a.focus();"" >
 < br >< br >< table width = ""100%"" >< tr >< td align = ""right"" >< a href = ""?s=905&n=14321"" > restart </ a > &nbsp;< a href = ""/"" > home </ a ></ right ></ td ></ tr ></ table >< table width = ""100%"" >< tr >< td width = ""80%"" valign = ""top"" >
                                                 < p class=""status""> BEDROOM TIME:  9:05 AM</p><br>
<br>
The phone rings.<br>
<br>
Oh, no -- how long have you been asleep? Sure, it was a tough night, but-- This is bad.This is very bad.<br>
<br>
The phone rings.<br>
<br>
  -----<br>
<b>9:05</b> by Adam Cadre<br>
Version 1.01 (10 February 2000) / Serial number 9502<br>
Written in Inform 6.21, library 6/10<br>
  -----<br>
<br>
<b>BEDROOM</b> (on the bed)<br>
This bedroom is extremely spare, with dirty laundry scattered haphazardly all over the floor.Cleaner clothing is to be found in the dresser. A bathroom lies to the south, while a door to the east leads to the living room.<br>
<br>
On the endtable are a telephone, a wallet (which is closed) and some keys.<br>
<br>
The phone rings.</td><td valign = ""top"" align=""right""></td></tr></table><br>
<br>
<form name = ""f"" method=""post""><input id = ""s"" type=""hidden"" name=""s"" value=""905""><input id = ""x"" type=""hidden"" name=""x"" value=""Q25260300681412550"">&gt&nbsp;<input id = ""a"" type=""text"" name=""a"" style=""border: 0; width: 80%; font-size: 110%; color: #006000;""><input type = ""submit"" value=""Enter""></form>
<script src = ""http://www.google-analytics.com/urchin.js"" type=""text/javascript"">
</script>
<script type = ""text/javascript"" >
_uacct = ""UA-4654789-1"";
urchinTracker();
</script>
</body></html>
";
        }
    }
}
