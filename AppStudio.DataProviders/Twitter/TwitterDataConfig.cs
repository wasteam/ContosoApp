using AppStudio.DataProviders.OAuth;

namespace AppStudio.DataProviders.Twitter
{
    public class TwitterDataConfig
    {
        public string QueryType { get; set; }

        public string Query { get; set; }

        public OAuthTokens Tokens { get; set; }
    }
}
