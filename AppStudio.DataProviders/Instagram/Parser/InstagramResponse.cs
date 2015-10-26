using System.Collections.Generic;
using System.Linq;

namespace AppStudio.DataProviders.Instagram.Parser
{
    internal class InstagramResponse
    {
        public InstagramResponseItem[] data { get; set; }

        public IEnumerable<InstagramSchema> ToSchema()
        {
            return this.data.Select(d => d.ToSchema());
        }
    }
}
