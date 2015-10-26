using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.DataProviders.YouTube.Parser
{
    internal class YouTubeChannelLookupResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public ContentDetails contentDetails { get; set; }
    }
}
