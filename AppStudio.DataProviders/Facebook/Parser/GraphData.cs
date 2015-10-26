using System;

namespace AppStudio.DataProviders.Facebook.Parser
{
    internal class GraphData
    {
        public string id { get; set; }
        public From from { get; set; }
        public string type { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public string message { get; set; }
        public string picture { get; set; }
        public string link { get; set; }
    }
}
