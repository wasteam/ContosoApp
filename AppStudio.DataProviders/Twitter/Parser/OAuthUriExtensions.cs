using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace AppStudio.DataProviders.Twitter.Parser
{
    internal static class OAuthUriExtensions
    {
        public static IDictionary<string, string> GetQueryParams(this Uri uri)
        {
            var result = new Dictionary<string, string>();

            foreach (Match item in Regex.Matches(uri.Query, @"(?<key>[^&?=]+)=(?<value>[^&?=]+)"))
            {
                result.Add(item.Groups["key"].Value, item.Groups["value"].Value);
            }

            return result;
        }

        public static string AbsoluteWithoutQuery(this Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query))
            {
                return uri.AbsoluteUri;
            }
            return uri.AbsoluteUri.Replace(uri.Query, string.Empty);
        }

        public static string Normalize(this Uri uri)
        {
            var result = new StringBuilder(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", uri.Scheme, uri.Host));
            if (!((uri.Scheme == "http" && uri.Port == 80) || (uri.Scheme == "https" && uri.Port == 443)))
            {
                result.Append(string.Concat(":", uri.Port));
            }
            result.Append(uri.AbsolutePath);

            return result.ToString();
        }
    }
}
