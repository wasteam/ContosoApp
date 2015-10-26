namespace AppStudio.DataProviders.Twitter.Parser
{
    public class TwitterTimeLineItem
    {
        public string created_at { get; set; }
        public string id_str { get; set; }
        public string text { get; set; }
        public TwitterUser user { get; set; }
    }
}
