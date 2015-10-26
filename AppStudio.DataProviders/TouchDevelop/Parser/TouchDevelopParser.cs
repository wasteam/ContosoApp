using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.TouchDevelop.Parser
{
    public class TouchDevelopParser : IParser<TouchDevelopSchema>
    {
        public IEnumerable<TouchDevelopSchema> Parse(string data)
        {
            var item = JsonConvert.DeserializeObject<TouchDevelopItem>(data);

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            yield return new TouchDevelopSchema
            {
                _id = item.id,
                Time = epoch.AddSeconds(item.time),
                Url = item.url,
                Name = Utility.DecodeHtml(item.name),
                Description = Utility.DecodeHtml(item.description),
                IconUrl = item.iconurl,
                IconBackground = item.iconbackground,
                UserId = item.userid,
                UserName = Utility.DecodeHtml(item.username),
                UserHasPicture = item.userhaspicture,
                UserScore = item.userscore,
                ScreenshotThumbUrl = item.screenshotthumburl,
                CumulativePositiveReviews = item.cumulativepositivereviews
            };
        }
    }
}
