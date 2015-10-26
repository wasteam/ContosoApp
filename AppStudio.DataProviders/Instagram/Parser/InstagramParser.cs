using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.Instagram.Parser
{
    public class InstagramParser : IParser<InstagramSchema>
    {
        public IEnumerable<InstagramSchema> Parse(string data)
        {
            var response = JsonConvert.DeserializeObject<InstagramResponse>(data);
            return response.ToSchema();
        }
    }
}
