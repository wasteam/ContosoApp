using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.YouTube.Parser
{
    public class YouTubeSearchParser : IParser<YouTubeSchema>
    {
        public IEnumerable<YouTubeSchema> Parse(string data)
        {
            Collection<YouTubeSchema> resultToReturn = new Collection<YouTubeSchema>();
            var searchList = JsonConvert.DeserializeObject<YouTubeResult<YouTubeSearchResult>>(data);
            if (searchList != null && searchList.items != null)
            {
                foreach (var item in searchList.items)
                {
                    resultToReturn.Add(new YouTubeSchema
                    {
                        _id = item.id.videoId,
                        Title = item.snippet.title,
                        ImageUrl = item.snippet.thumbnails != null ? item.snippet.thumbnails.high.url : string.Empty,
                        Summary = item.snippet.description,
                        Published = item.snippet.publishedAt,
                        VideoId = item.id.videoId
                    });
                }
            }

            return resultToReturn;
        }
    }
}
