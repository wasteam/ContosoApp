using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace AppStudio.DataProviders.Twitter.Parser
{
    internal class OAuthEncoder
    {
        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var result = Uri.EscapeDataString(value);

            // UrlEncode escapes with lowercase characters (e.g. %2f) but oAuth needs %2F
            result = Regex.Replace(result, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());

            // these characters are not escaped by UrlEncode() but needed to be escaped
            result = result
                        .Replace("(", "%28")
                        .Replace(")", "%29")
                        .Replace("$", "%24")
                        .Replace("!", "%21")
                        .Replace("*", "%2A")
                        .Replace("'", "%27");

            // these characters are escaped by UrlEncode() but will fail if unescaped!
            result = result.Replace("%7E", "~");

            return result;
        }

        public static string UrlEncode(IEnumerable<OAuthParameter> parameters)
        {
            string rawUrl = string.Join("&", parameters.OrderBy(p => p.Key).Select(p => p.ToString()).ToArray());
            return UrlEncode(rawUrl);
        }

        public static string GenerateHash(string input, string key)
        {
            MacAlgorithmProvider mac = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            CryptographicKey cryptoKey = mac.CreateKey(keyMaterial);
            IBuffer hash = CryptographicEngine.Sign(cryptoKey, CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8));
            return CryptographicBuffer.EncodeToBase64String(hash);
        }
    }
}
