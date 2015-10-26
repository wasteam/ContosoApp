using AppStudio.DataProviders.OAuth;

namespace AppStudio.DataProviders.Instagram
{
    public class InstagramDataConfig
    {
        public string QueryType { get; set; }
        
        public string Query { get; set; }

        public OAuthTokens Tokens { get; set; }
    }
}
