using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.DataProviders.OAuth;

namespace AppStudio.DataProviders.YouTube
{
    public class YouTubeDataConfig
    {
        public string QueryType { get; set; }
        
        public string Query { get; set; }

        public OAuthTokens Tokens { get; set; }
    }
}
