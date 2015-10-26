using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using AppStudio.DataProviders.Core;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.Facebook.Parser
{
    public class FacebookParser : IParser<FacebookSchema>
    {
        public IEnumerable<FacebookSchema> Parse(string data)
        {
            Collection<FacebookSchema> resultToReturn = new Collection<FacebookSchema>();
            var searchList = JsonConvert.DeserializeObject<FacebookGraphResponse>(data);
            foreach (var i in searchList.data.Where(w => !string.IsNullOrEmpty(w.message) && !string.IsNullOrEmpty(w.link)).OrderByDescending(o => o.created_time))
            {
                var item = new FacebookSchema();
                item._id = i.id;
                item.Author = i.from.name;
                item.PublishDate = i.created_time;
                item.Title = Utility.DecodeHtml(i.message);
                item.Summary = Utility.DecodeHtml(i.message);
                item.Content = i.message;
                item.ImageUrl = ConvertImageUrlFromParameter(i.picture);
                item.FeedUrl = i.link;
                resultToReturn.Add(item);
            }

            return resultToReturn;
        }

        private string ConvertImageUrlFromParameter(string imageUrl)
        {
            string parsedImageUrl = null;
            try
            {
                if (!string.IsNullOrEmpty(imageUrl) && imageUrl.IndexOf("url=") > 0)
                {
                    Uri imageUri = new Uri(imageUrl);
                    var imageUriQuery = imageUri.Query.Replace("?", "").Replace("&amp;", "&");

                    var imageUriQueryParameters = imageUriQuery.Split('&').Select(q => q.Split('='))
                           .Where(s => s != null && s.Length >= 2)
                           .ToDictionary(k => k[0], v => v[1]);

                    parsedImageUrl = WebUtility.UrlDecode(imageUriQueryParameters["url"]);
                }
                else if (!string.IsNullOrEmpty(imageUrl))
                {
                    parsedImageUrl = WebUtility.HtmlDecode(imageUrl);
                }
            }
            catch
            {
            }

            return parsedImageUrl;
        }
    }
}
