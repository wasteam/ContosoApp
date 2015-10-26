namespace AppStudio.DataProviders.YouTube.Parser
{
    internal class YouTubePlaylistResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public YouTubePlaylistSnippet snippet { get; set; }
    }
}
