using System.Collections.Generic;
using System.Linq;
using AppStudio.DataProviders.Rss;
using AppStudio.DataProviders.Rss.Parser;

namespace AppStudio.DataProviders.Bing.Parser
{
    public class BingParser : IParser<BingSchema>
    {
        public IEnumerable<BingSchema> Parse(string data)
        {
            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> syndicationItems = rssParser.Parse(data);
            return (from r in syndicationItems
                    select new BingSchema()
                    {
                        _id = r._id,
                        Title = r.Title,
                        Summary = r.Summary,
                        Link = r.FeedUrl,
                        Published = r.PublishDate
                    });
        }
    }
}
