using System.Collections.Generic;

namespace AppStudio.DataProviders.Html.Parser
{
    public class HtmlParser : IParser<HtmlSchema>
    {
        public IEnumerable<HtmlSchema> Parse(string data)
        {
            return new HtmlSchema[] { new HtmlSchema { _id = "html", Content = data } };
        }
    }
}
