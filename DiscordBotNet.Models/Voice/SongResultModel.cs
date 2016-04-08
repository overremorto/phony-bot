using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotNet.Models.Voice
{
    public class SongResultModel
    {
        public string SongName { get; set; }

        public string ArtistName { get; set; }

        public string[] Lyrics { get; set; }
    }
}
