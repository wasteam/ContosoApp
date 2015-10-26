using System.Collections.Generic;
using System.Linq;
using AppStudio.DataProviders.Rss;
using AppStudio.DataProviders.Rss.Parser;

namespace AppStudio.DataProviders.Flickr.Parser
{
    public class FlickrParser : IParser<FlickrSchema>
    {
        public IEnumerable<FlickrSchema> Parse(string data)
        {
            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> flickrItems = rssParser.Parse(data);
            return (from r in flickrItems
                    select new FlickrSchema()
                    {
                        _id = r._id,
                        Title = r.Title,
                        Summary = r.Summary,
                        // Change medium images to large
                        ImageUrl = r.ImageUrl.Replace("_m.jpg", "_b.jpg"),
                        Published = r.PublishDate,
                        FeedUrl = r.FeedUrl
                    });
        }
    }
}
