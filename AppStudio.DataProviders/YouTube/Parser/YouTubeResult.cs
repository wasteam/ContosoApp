using System.Collections.Generic;

namespace AppStudio.DataProviders.YouTube.Parser
{
    internal class YouTubeResult<T>
    {
        public string error { get; set; }
        public List<T> items { get; set; }
    }
}
