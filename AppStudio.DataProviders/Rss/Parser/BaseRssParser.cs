using System.Collections.Generic;
using System.Xml.Linq;

namespace AppStudio.DataProviders.Rss.Parser
{
    internal abstract class BaseRssParser
    {
        private static readonly XNamespace NsMedia = "http://search.yahoo.com/mrss/";
        private static readonly XNamespace NsItunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        /// <summary>
        /// Get the feed type: Rss, Atom or Unknown
        /// </summary>
        /// <param name="rss"></param>
        /// <returns></returns>
        public static RssType GetFeedType(XDocument doc)
        {
            if (doc.Root == null)
            {
                //AppLogs.WriteError("AtomReader.LoadFeed", "Not supported type");
                return RssType.Unknown;
            }
            XNamespace defaultNamespace = doc.Root.GetDefaultNamespace();
            return defaultNamespace.NamespaceName.EndsWith("Atom") ? RssType.Atom : RssType.Rss;
        }

        /// <summary>
        /// Abstract method to be override by specific implementations of the reader.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public abstract IEnumerable<RssSchema> LoadFeed(XDocument doc);
    }
}
