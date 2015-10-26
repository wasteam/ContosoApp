using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AppStudio.DataProviders.OAuth;

namespace AppStudio.DataProviders.Twitter.Parser
{
    internal class OAuthRequestBuilder
    {
        public const string Realm = "Twitter API";
        public const string Verb = "GET";

        public Uri EncodedRequestUri { get; private set; }
        public Uri RequestUriWithoutQuery { get; private set; }
        public IEnumerable<OAuthParameter> QueryParams { get; private set; }
        public OAuthParameter Version { get; private set; }
        public OAuthParameter Nonce { get; private set; }
        public OAuthParameter Timestamp { get; private set; }
        public OAuthParameter SignatureMethod { get; private set; }
        public OAuthParameter ConsumerKey { get; private set; }
        public OAuthParameter ConsumerSecret { get; private set; }
        public OAuthParameter Token { get; private set; }
        public OAuthParameter TokenSecret { get; private set; }
        public OAuthParameter Signature 
        { 
            get
            {
                return new OAuthParameter("oauth_signature", GenerateSignature());
            }
        }
        public string AuthorizationHeader
        {
            get
            {
                return GenerateAuthorizationHeader();
            }
        }

        public OAuthRequestBuilder(Uri requestUri, OAuthTokens tokens)
        {
            RequestUriWithoutQuery = new Uri(requestUri.AbsoluteWithoutQuery());

            QueryParams = requestUri.GetQueryParams()
                                        .Select(p => new OAuthParameter(p.Key, p.Value))
                                        .ToList();

            EncodedRequestUri = GetEncodedUri(requestUri, QueryParams);

            Version = new OAuthParameter("oauth_version", "1.0");
            Nonce = new OAuthParameter("oauth_nonce", GenerateNonce());
            Timestamp = new OAuthParameter("oauth_timestamp", GenerateTimeStamp());
            SignatureMethod = new OAuthParameter("oauth_signature_method", "HMAC-SHA1");
            ConsumerKey = new OAuthParameter("oauth_consumer_key", tokens["ConsumerKey"]);
            ConsumerSecret = new OAuthParameter("oauth_consumer_secret", tokens["ConsumerSecret"]);
            Token = new OAuthParameter("oauth_token", tokens["AccessToken"]);
            TokenSecret = new OAuthParameter("oauth_token_secret", tokens["AccessTokenSecret"]);
        }

        private static Uri GetEncodedUri(Uri requestUri, IEnumerable<OAuthParameter> parameters)
        {
            StringBuilder requestParametersBuilder = new StringBuilder(requestUri.AbsoluteWithoutQuery());
            if (parameters.Count() > 0)
            {
                requestParametersBuilder.Append("?");

                foreach (var queryParam in parameters)
                {
                    requestParametersBuilder.AppendFormat("{0}&", queryParam.ToString());
                }

                requestParametersBuilder.Remove(requestParametersBuilder.Length - 1, 1);
            }

            return new Uri(requestParametersBuilder.ToString());
        }

        private static string GenerateNonce()
        {
            return new Random()
                        .Next(123400, int.MaxValue)
                        .ToString("X", CultureInfo.InvariantCulture);
        }

        private static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
        }

        private string GenerateSignature()
        {
            string signatureBaseString = string.Format(
                CultureInfo.InvariantCulture,
                "GET&{0}&{1}",
                OAuthEncoder.UrlEncode(RequestUriWithoutQuery.Normalize()),
                OAuthEncoder.UrlEncode(GetSignParameters()));

            string key = string.Format(
                CultureInfo.InvariantCulture,
                "{0}&{1}",
                OAuthEncoder.UrlEncode(ConsumerSecret.Value),
                OAuthEncoder.UrlEncode(TokenSecret.Value));

            return OAuthEncoder.GenerateHash(signatureBaseString, key);
        }

        private string GenerateAuthorizationHeader()
        {
            StringBuilder authHeaderBuilder = new StringBuilder();

            authHeaderBuilder.AppendFormat("OAuth realm=\"{0}\",", Realm);
            authHeaderBuilder.Append(string.Join(",", GetAuthHeaderParameters().OrderBy(p => p.Key).Select(p => p.ToString(true)).ToArray()));
            authHeaderBuilder.AppendFormat(",{0}", Signature.ToString(true));

            return authHeaderBuilder.ToString();
        }

        private IEnumerable<OAuthParameter> GetSignParameters()
        {
            foreach (var queryParam in QueryParams)
            {
                yield return queryParam;
            }
            yield return Version;
            yield return Nonce;
            yield return Timestamp;
            yield return SignatureMethod;
            yield return ConsumerKey;
            yield return Token;
        }

        private IEnumerable<OAuthParameter> GetAuthHeaderParameters()
        {
            yield return Version;
            yield return Nonce;
            yield return Timestamp;
            yield return SignatureMethod;
            yield return ConsumerKey;
            yield return Token;
        }
    }
}
