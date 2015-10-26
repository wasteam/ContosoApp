using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.OAuth;

namespace AppStudio.DataProviders.Twitter.Parser
{
    internal class OAuthRequest
    {
        public async Task<string> ExecuteAsync(Uri requestUri, OAuthTokens tokens)
        {
            var request = CreateRequest(requestUri, tokens);
            var response = await request.GetResponseAsync();

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

        private static WebRequest CreateRequest(Uri requestUri, OAuthTokens tokens)
        {
            var requestBuilder = new OAuthRequestBuilder(requestUri, tokens);

            var request = (HttpWebRequest)WebRequest.Create(requestBuilder.EncodedRequestUri);

            request.UseDefaultCredentials = true;
            request.Method = OAuthRequestBuilder.Verb;
            request.Headers["Authorization"] = requestBuilder.AuthorizationHeader;

            return request;
        }
    }
}
