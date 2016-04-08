using System;
using DiscordBotNet.Module.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiscordBotNet.Tests.Command
{
    [TestClass]
    public class SingCommandTest
    {
        [TestMethod]
        public void GetSongFromLyricsDotComTest()
        {
            var command = new SingCommand();
            var lyrics = command.GetSongFromLyricsDotCom("beat it michael jackson");
            Assert.IsTrue(lyrics != null);
            Assert.AreEqual("Michael Jackson", lyrics.ArtistName);
            Assert.AreEqual("Beat It", lyrics.SongName);
            Assert.AreEqual(54, lyrics.Lyrics.Length);
        }
    }
}
