using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.YouTube.Parser
{
    public class YouTubePlaylistParser : IParser<YouTubeSchema>
    {
        public IEnumerable<YouTubeSchema> Parse(string data)
        {
            Collection<YouTubeSchema> resultToReturn = new Collection<YouTubeSchema>();
            var playlist = JsonConvert.DeserializeObject<YouTubeResult<YouTubePlaylistResult>>(data);
            if (playlist != null && playlist.items != null)
            {
                foreach (var item in playlist.items)
                {
                    resultToReturn.Add(new YouTubeSchema()
                    {
                        _id = item.id,
                        Title = item.snippet.title,
                        ImageUrl = item.snippet.thumbnails != null ? item.snippet.thumbnails.high.url : string.Empty,
                        Summary = item.snippet.description,
                        Published = item.snippet.publishedAt,
                        VideoId = item.snippet.resourceId.videoId
                    });
                }
            }

            return resultToReturn;

        }
    }
}
