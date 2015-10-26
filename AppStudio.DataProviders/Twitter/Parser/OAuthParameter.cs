using System.Globalization;

namespace AppStudio.DataProviders.Twitter.Parser
{
    internal class OAuthParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public OAuthParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool withQuotes)
        {
            string format = null;
            if (withQuotes)
            {
                format = "{0}=\"{1}\"";
            }
            else
            {
                format = "{0}={1}";
            }
            return string.Format(CultureInfo.InvariantCulture, format, OAuthEncoder.UrlEncode(Key), OAuthEncoder.UrlEncode(Value));
        }
    }
}
