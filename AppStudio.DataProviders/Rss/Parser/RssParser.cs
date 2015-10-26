using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AppStudio.DataProviders.Rss.Parser;

namespace AppStudio.DataProviders.Rss.Parser
{
    public class RssParser : IParser<RssSchema>
    {
        public IEnumerable<RssSchema> Parse(string data)
        {
            var doc = XDocument.Parse(data);
            var type = BaseRssParser.GetFeedType(doc);

            BaseRssParser rssParser;
            if (type == RssType.Rss)
            {
                rssParser = new Rss2Parser();
            }
            else
            {
                rssParser = new AtomParser();
            }

            return rssParser.LoadFeed(doc);
        }
    }
}
