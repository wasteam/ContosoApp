using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Facebook.Parser;
using AppStudio.DataProviders.InternetClient;

namespace AppStudio.DataProviders.Facebook
{
    public class FacebookDataProvider : DataProviderBase<FacebookDataConfig, FacebookSchema>
    {
        private const string BaseUrl = @"https://graph.facebook.com/v2.2";

        public FacebookDataProvider(FacebookDataConfig config)
            : base(config, new FacebookParser())
        {
        }

        public FacebookDataProvider(FacebookDataConfig config, IParser<FacebookSchema> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<FacebookSchema>> LoadDataAsync()
        {
            InternetRequestSettings settings = new InternetRequestSettings
            {
                RequestedUri = this.GetPageUrl()
            };

            var requestResult = await InternetRequest.DownloadAsync(settings);
            if (requestResult.Success)
            {
                return _parser.Parse(requestResult.Result);
            }

            if (requestResult.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException();
        }

        private Uri GetPageUrl()
        {
            return new Uri(string.Format("{0}/{1}/posts?&access_token={2}|{3}", BaseUrl, _config.UserId, _config.Tokens["AppId"], _config.Tokens["AppSecret"]), UriKind.Absolute);
        }
    }
}
