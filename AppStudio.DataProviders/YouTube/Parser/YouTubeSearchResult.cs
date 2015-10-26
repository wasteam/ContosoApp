using System;

namespace AppStudio.DataProviders.YouTube.Parser
{
    internal class YouTubeSearchResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public YouTubeSearchId id { get; set; }
        public YouTubeSearchSnippet snippet { get; set; }
    }
}
