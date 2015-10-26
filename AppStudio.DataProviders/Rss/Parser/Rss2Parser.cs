using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using AppStudio.DataProviders.Core;

namespace AppStudio.DataProviders.Rss.Parser
{
    /// <summary>
    /// Rss reader implementation to parse Rss content.
    /// </summary>
    internal class Rss2Parser : BaseRssParser
    {
        private static readonly XNamespace NsRdfNamespaceUri = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        private static readonly XNamespace NsRdfElementsNamespaceUri = "http://purl.org/dc/elements/1.1/";
        private static readonly XNamespace NsRdfContentNamespaceUri = "http://purl.org/rss/1.0/modules/content/";

        /// <summary>
        /// THis override load and parses the document and return a list of RssSchema values.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public override IEnumerable<RssSchema> LoadFeed(XDocument doc)
        {
            bool isRDF = false;
            var feed = new Collection<RssSchema>();
            XNamespace defaultNamespace = string.Empty;

            if (doc.Root != null)
            {
                isRDF = doc.Root.Name == (NsRdfNamespaceUri + "RDF");
                defaultNamespace = doc.Root.GetDefaultNamespace();
            }

            foreach (var item in doc.Descendants(defaultNamespace + "item"))
            {
                var rssItem = isRDF ? ParseRDFItem(item) : ParseRssItem(item);
                feed.Add(rssItem);
            }
            return feed;
        }

        /// <summary>
        /// RSS all versions
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static RssSchema ParseItem(XElement item)
        {
            var rssItem = new RssSchema();
            rssItem.Title = Utility.DecodeHtml(item.GetSafeElementString("title").Trim());
            rssItem.FeedUrl = item.GetSafeElementString("link");

            rssItem.Author = GetItemAuthor(item);

            string description = item.GetSafeElementString("encoded", NsRdfContentNamespaceUri);
            if (string.IsNullOrEmpty(description))
            {
                description = item.GetSafeElementString("description");
                if (string.IsNullOrEmpty(description))
                {
                    description = item.GetSafeElementString("content");
                }
            }

            rssItem.Summary = RssHelper.SanitizeString(Utility.DecodeHtml(description).Trim().Truncate(500, true));
            rssItem.Content = RssHelper.SanitizeString(description);

            string id = item.GetSafeElementString("guid").Trim();
            if (string.IsNullOrEmpty(id))
            {
                id = item.GetSafeElementString("id").Trim();
                if (string.IsNullOrEmpty(id))
                {
                    id = rssItem.FeedUrl;
                }
            }
            rssItem._id = id;

            return rssItem;
        }

        /// <summary>
        /// RSS version 1.0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private RssSchema ParseRDFItem(XElement item)
        {
            XNamespace ns = "http://search.yahoo.com/mrss/";
            var rssItem = ParseItem(item);

            rssItem.PublishDate = item.GetSafeElementDate("date", NsRdfElementsNamespaceUri);

            string image = item.GetSafeElementString("image");
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImage();
            }
            if (string.IsNullOrEmpty(image) && item.Elements(ns + "thumbnail").LastOrDefault() != null)
            {

                var element = item.Elements(ns + "thumbnail").Last();
                image = element.Attribute("url").Value;
            }
            if (string.IsNullOrEmpty(image) && item.ToString().Contains("thumbnail"))
            {
                image = item.GetSafeElementString("thumbnail");
            }

            rssItem.ImageUrl = image;

            return rssItem;
        }

        /// <summary>
        /// RSS version 2.0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static RssSchema ParseRssItem(XElement item)
        {
            XNamespace ns = "http://search.yahoo.com/mrss/";
            var rssItem = ParseItem(item);

            rssItem.PublishDate = item.GetSafeElementDate("pubDate");

            string image = item.GetSafeElementString("image");
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImageFromEnclosure();
            }
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImage();
            }
            if (string.IsNullOrEmpty(image) && item.Elements(ns + "thumbnail").LastOrDefault() != null)
            {
                var element = item.Elements(ns + "thumbnail").Last();
                image = element.Attribute("url").Value;
            }
            if (string.IsNullOrEmpty(image) && item.ToString().Contains("thumbnail"))
            {
                image = item.GetSafeElementString("thumbnail");
            }

            rssItem.ImageUrl = image;

            return rssItem;
        }

        private static string GetItemAuthor(XElement item)
        {
            var content = item.GetSafeElementString("creator", NsRdfElementsNamespaceUri).Trim();
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("author");
            }

            return content;
        }

    }
}
