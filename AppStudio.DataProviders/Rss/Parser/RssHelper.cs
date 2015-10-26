using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AppStudio.DataProviders.Rss.Parser
{
    /// <summary>
    /// Class with utilities for Rss related works.
    /// </summary>
    internal static class RssHelper
    {
        private const string Imgpattern = @"<(?<Tag_Name>(a)|img)\b[^>]*?\b(?<URL_Type>(?(1)href|src))\s*=\s*(?:""(?<URL>(?:\\""|[^""])*)""|'(?<URL>(?:\\'|[^'])*)')";
        private const string Urlpattern = @"(www.+|http.+)([\s]|$)";

        /// <summary>
        /// Removes \t characters in the string and trim additional space and carriage returns.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SanitizeString(string text)
        {
            var textArray = text.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            string sanitizedText = string.Empty;
            foreach (var item in textArray.ToList())
            {
                sanitizedText += item.Trim();
            }

            sanitizedText = string.Join(" ", Regex.Split(sanitizedText, @"(?:\r\n|\n|\r)"));

            return sanitizedText;
        }

        /// <summary>
        /// Get item date from xelement and element name.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static DateTime GetSafeElementDate(this XElement item, string elementName)
        {
            return GetSafeElementDate(item, elementName, item.GetDefaultNamespace());
        }

        /// <summary>
        /// Get item date from xelement, element name and namespace.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static DateTime GetSafeElementDate(this XElement item, string elementName, XNamespace xNamespace)
        {
            DateTime date;
            XElement element = item.Element(xNamespace + elementName);
            if (element == null)
                return DateTime.Now;
            if (TryParseDateTime(element.Value, out date))
                return date;
            return DateTime.Now;
        }

        /// <summary>
        /// Get item string value for xelement and element name.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static string GetSafeElementString(this XElement item, string elementName)
        {
            if (item == null) return String.Empty;
            return GetSafeElementString(item, elementName, item.GetDefaultNamespace());
        }

        /// <summary>
        /// Get item string value for xelement, element name and namespace.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static string GetSafeElementString(this XElement item, string elementName, XNamespace xNamespace)
        {
            if (item == null) return String.Empty;
            XElement value = item.Element(xNamespace + elementName);
            if (value != null)
                return value.Value;
            return String.Empty;
        }

        /// <summary>
        /// Get feed url to see full original information.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public static string GetLink(this XElement item, string rel)
        {
            IEnumerable<XElement> links = item.Elements(item.GetDefaultNamespace() + "link");
            IEnumerable<string> link = from l in links
                                       let xAttribute = l.Attribute("rel")
                                       where xAttribute != null && xAttribute.Value == rel
                                       let attribute = l.Attribute("href")
                                       where attribute != null
                                       select attribute.Value;
            if (!link.Any() && links.Any())
                return links.FirstOrDefault().Attributes().First().Value;

            return link.FirstOrDefault();
        }

        /// <summary>
        /// Get feed image.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetImage(this XElement item)
        {
            string feedDataImage = null;
            try
            {
                feedDataImage = GetImagesInHTMLString(item.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(feedDataImage) && feedDataImage.EndsWith("'"))
                {
                    feedDataImage = feedDataImage.Remove(feedDataImage.Length - 1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return feedDataImage;
        }

        /// <summary>
        /// Get the item image from the enclosure element http://www.w3schools.com/rss/rss_tag_enclosure.asp
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetImageFromEnclosure(this XElement item)
        {
            string feedDataImage = null;
            try
            {
                XElement element = item.Element(item.GetDefaultNamespace() + "enclosure");
                if (element == null)
                    return string.Empty;

                var typeAttribute = element.Attribute("type");
                if (typeAttribute != null &&
                    !string.IsNullOrEmpty(typeAttribute.Value) &&
                    typeAttribute.Value.StartsWith("image"))
                {
                    var urlAttribute = element.Attribute("url");
                    feedDataImage = (urlAttribute != null && !string.IsNullOrEmpty(urlAttribute.Value)) ?
                        urlAttribute.Value : string.Empty;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return feedDataImage;
        }

        /// <summary>
        /// Tries to parse the original string to a datetime format.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseDateTime(string s, out DateTime result)
        {
            if (DateTime.TryParse(s, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces, out result))
            {
                return true;
            }

            int tzIndex = s.LastIndexOf(" ");
            if (tzIndex >= 0)
            {
                string tz = s.Substring(tzIndex, s.Length - tzIndex);
                string offset = TimeZoneToOffset(tz);
                if (offset != null)
                {
                    string offsetDate = string.Format("{0} {1}", s.Substring(0, tzIndex), offset);
                    return TryParseDateTime(offsetDate, out result);
                }
            }

            result = default(DateTime);
            return false;
        }

        /// <summary>
        /// Calculate and return timezone.
        /// </summary>
        /// <param name="tz"></param>
        /// <returns></returns>
        public static string TimeZoneToOffset(string tz)
        {
            if (tz == null)
                return null;

            tz = tz.ToUpper().Trim();

            if (TimeZones.ContainsKey(tz))
                return TimeZones[tz].First();
            else
                return null;
        }

        private static IEnumerable<string> GetImagesInHTMLString(string htmlString)
        {
            var images = new List<string>();
            const string pattern = Imgpattern;
            var rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(htmlString);

            for (int i = 0, l = matches.Count; i < l; i++)
            {
                if (matches[i].Value.Contains(".jpg") || matches[i].Value.Contains(".png"))
                {
                    var ms = Regex.Matches(matches[i].Value, Urlpattern);
                    if (ms.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ms[0].Value))
                            images.Add(ms[0].Value.Replace("\"", string.Empty));
                    }
                }
            }

            return images;
        }

        private static Dictionary<string, string[]> TimeZones = new Dictionary<string, string[]>
        {
            {"ACDT", new string[] { "-1030", "Australian Central Daylight" }},
            {"ACST", new string[] { "-0930", "Australian Central Standard"}},
            {"ADT", new string[] { "+0300", "(US) Atlantic Daylight"}},
            {"AEDT", new string[] { "-1100", "Australian East Daylight"}},
            {"AEST", new string[] { "-1000", "Australian East Standard"}},
            {"AHDT", new string[] { "+0900", ""}},
            {"AHST", new string[] { "+1000", ""}},
            {"AST", new string[] { "+0400", "(US) Atlantic Standard"}},
            {"AT", new string[] { "+0200", "Azores"}},
            {"AWDT", new string[] { "-0900", "Australian West Daylight"}},
            {"AWST", new string[] { "-0800", "Australian West Standard"}},
            {"BAT", new string[] { "-0300", "Bhagdad"}},
            {"BDST", new string[] { "-0200", "British Double Summer"}},
            {"BET", new string[] { "+1100", "Bering Standard"}},
            {"BST", new string[] { "+0300", "Brazil Standard"}},
            {"BT", new string[] { "-0300", "Baghdad"}},
            {"BZT2", new string[] { "+0300", "Brazil Zone 2"}},
            {"CADT", new string[] { "-1030", "Central Australian Daylight"}},
            {"CAST", new string[] { "-0930", "Central Australian Standard"}},
            {"CAT", new string[] { "+1000", "Central Alaska"}},
            {"CCT", new string[] { "-0800", "China Coast"}},
            {"CDT", new string[] { "+0500", "(US) Central Daylight"}},
            {"CED", new string[] { "-0200", "Central European Daylight"}},
            {"CET", new string[] { "-0100", "Central European"}},
            {"CST", new string[] { "+0600", "(US) Central Standard"}},
            {"EAST", new string[] { "-1000", "Eastern Australian Standard"}},
            {"EDT", new string[] { "+0400", "(US) Eastern Daylight"}},
            {"EED", new string[] { "-0300", "Eastern European Daylight"}},
            {"EET", new string[] { "-0200", "Eastern Europe"}},
            {"EEST", new string[] { "-0300", "Eastern Europe Summer"}},
            {"EST", new string[] { "+0500", "(US) Eastern Standard"}},
            {"FST", new string[] { "-0200", "French Summer"}},
            {"FWT", new string[] { "-0100", "French Winter"}},
            {"GMT", new string[] { "+0000", "Greenwich Mean"}},
            {"GST", new string[] { "-1000", "Guam Standard"}},
            {"HDT", new string[] { "+0900", "Hawaii Daylight"}},
            {"HST", new string[] { "+1000", "Hawaii Standard"}},
            {"IDLE", new string[] { "-1200", "Internation Date Line East"}},
            {"IDLW", new string[] { "+1200", "Internation Date Line West"}},
            {"IST", new string[] { "-0530", "Indian Standard"}},
            {"IT", new string[] { "-0330", "Iran"}},
            {"JST", new string[] { "-0900", "Japan Standard"}},
            {"JT", new string[] { "-0700", "Java"}},
            {"MDT", new string[] { "+0600", "(US) Mountain Daylight"}},
            {"MED", new string[] { "-0200", "Middle European Daylight"}},
            {"MET", new string[] { "-0100", "Middle European"}},
            {"MEST", new string[] { "-0200", "Middle European Summer"}},
            {"MEWT", new string[] { "-0100", "Middle European Winter"}},
            {"MST", new string[] { "+0700", "(US) Mountain Standard"}},
            {"MT", new string[] { "-0800", "Moluccas"}},
            {"NDT", new string[] { "+0230", "Newfoundland Daylight"}},
            {"NFT", new string[] { "+0330", "Newfoundland"}},
            {"NT", new string[] { "+1100", "Nome"}},
            {"NST", new string[] { "-0630", "North Sumatra"}},
            {"NZ", new string[] { "-1100", "New Zealand "}},
            {"NZST", new string[] { "-1200", "New Zealand Standard"}},
            {"NZDT", new string[] { "-1300", "New Zealand Daylight"}},
            {"NZT", new string[] { "-1200", "New Zealand"}},
            {"PDT", new string[] { "+0700", "(US) Pacific Daylight"}},
            {"PST", new string[] { "+0800", "(US) Pacific Standard"}},
            {"ROK", new string[] { "-0900", "Republic of Korea"}},
            {"SAD", new string[] { "-1000", "South Australia Daylight"}},
            {"SAST", new string[] { "-0900", "South Australia Standard"}},
            {"SAT", new string[] { "-0900", "South Australia Standard"}},
            {"SDT", new string[] { "-1000", "South Australia Daylight"}},
            {"SST", new string[] { "-0200", "Swedish Summer"}},
            {"SWT", new string[] { "-0100", "Swedish Winter"}},
            {"USZ3", new string[] { "-0400", "USSR Zone 3"}},
            {"USZ4", new string[] { "-0500", "USSR Zone 4"}},
            {"USZ5", new string[] { "-0600", "USSR Zone 5"}},
            {"USZ6", new string[] { "-0700", "USSR Zone 6"}},
            {"UT", new string[] { "+0000", "Universal Coordinated"}},
            {"UTC", new string[] { "+0000", "Universal Coordinated"}},
            {"UZ10", new string[] { "-1100", "USSR Zone 10"}},
            {"WAT", new string[] { "+0100", "West Africa"}},
            {"WET", new string[] { "+0000", "West European"}},
            {"WST", new string[] { "-0800", "West Australian Standard"}},
            {"YDT", new string[] { "+0800", "Yukon Daylight"}},
            {"YST", new string[] { "+0900", "Yukon Standard"}},
            {"ZP4", new string[] { "-0400", "USSR Zone 3"}},
            {"ZP5", new string[] { "-0500", "USSR Zone 4"}},
            {"ZP6", new string[] { "-0600", "USSR Zone 5"}}
        };
    }
}
