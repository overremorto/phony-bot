using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotNet.Models.Youtube
{
    public class YoutubeResult
    {
        public string Kind { get; set; }

        public string ETag { get; set; }

        public string NextPageToken { get; set; }

        public string RegionCode { get; set; }

        public YoutubeResultPageInfo PageInfo { get; set; }

        public IEnumerable<YoutubeResultItem> Items { get; set; }
    }

    public class YoutubeResultPageInfo
    {
        public int TotalResults { get; set; }

        public int resultsPerPage { get; set; }
    }

    public class YoutubeResultItem
    {
        public string Kind { get; set; }

        public string ETag { get; set; }

        public YoutubeResultItemId Id { get; set; }

        public YoutubeResultItemSnippet Snippet { get; set; }
    }

    public class YoutubeResultItemId
    {
        public string Kind { get; set; }

        public string VideoId { get; set; }
    }

    public class YoutubeResultItemSnippet
    {
        public DateTime PublishedAt { get; set; }

        public string ChannelId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ChannelTitle { get; set; }

        public string LiveBroadcastContent { get; set; }

        public YoutubeResultItemSnippetThumbnailCollection Thumbnails { get; set; }
    }

    public class YoutubeResultItemSnippetThumbnailCollection
    {
        public YoutubeResultItemSnippetThumbnail Default { get; set; }
        public YoutubeResultItemSnippetThumbnail Medium { get; set; }
        public YoutubeResultItemSnippetThumbnail Hight { get; set; }
    }

    public class YoutubeResultItemSnippetThumbnail
    {
        public string Url { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
