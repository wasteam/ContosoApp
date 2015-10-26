using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.Twitter.Parser
{
    public class TwitterTimelineParser : IParser<TwitterSchema>
    {
        public IEnumerable<TwitterSchema> Parse(string data)
        {
            var result = JsonConvert.DeserializeObject<TwitterTimeLineItem[]>(data);

            return result.Select(r => r.Parse()).ToList();
        }
    }
}
