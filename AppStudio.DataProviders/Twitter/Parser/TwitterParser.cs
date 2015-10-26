using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;

namespace AppStudio.DataProviders.Twitter.Parser
{
    internal static class TwitterParser
    {
        public static TwitterSchema Parse(this TwitterTimeLineItem item)
        {
            TwitterSchema twit = new TwitterSchema
            {
                _id = item.id_str,
                Text = Utility.DecodeHtml(item.text),
                CreationDateTime = TryParse(item.created_at)
            };

            if (item.user == null)
            {
                twit.UserId = string.Empty;
                twit.UserName = string.Empty;
                twit.UserScreenName = string.Empty;
                twit.UserProfileImageUrl = string.Empty;
                twit.Url = string.Empty;
            }
            else
            {
                twit.UserId = item.user.id_str;
                twit.UserName = Utility.DecodeHtml(item.user.name);
                twit.UserScreenName = string.Concat("@", Utility.DecodeHtml(item.user.screen_name));
                twit.UserProfileImageUrl = item.user.profile_image_url;
                twit.Url = string.Format("https://twitter.com/{0}/status/{1}", item.user.screen_name, item.id_str);
                if (!string.IsNullOrEmpty(twit.UserProfileImageUrl))
                {
                    twit.UserProfileImageUrl = twit.UserProfileImageUrl.Replace("_normal", string.Empty);
                }
            }

            return twit;
        }

        private static DateTime TryParse(string dateTime)
        {
            DateTime dt;
            if (!DateTime.TryParseExact(dateTime, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                dt = DateTime.Today;
            }

            return dt;
        }
    }
}
