using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.Twitter.Parser
{
    public class TwitterSearchParser : IParser<TwitterSchema>
    {
        public IEnumerable<TwitterSchema> Parse(string data)
        {
            var result = JsonConvert.DeserializeObject<TwitterSearchResult>(data);

            return result.statuses.Select(r => r.Parse()).ToList();
        }
    }
}
