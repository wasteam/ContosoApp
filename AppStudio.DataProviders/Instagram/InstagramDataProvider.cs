using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Instagram.Parser;
using AppStudio.DataProviders.InternetClient;

namespace AppStudio.DataProviders.Instagram
{
    public class InstagramDataProvider : DataProviderBase<InstagramDataConfig, InstagramSchema>
    {
        private const string URL = "https://api.instagram.com/v1/tags/{0}/media/recent?client_id={1}";
        private const string URLUserID = "https://api.instagram.com/v1/users/{0}/media/recent?client_id={1}";

        public InstagramDataProvider(InstagramDataConfig config)
            : base(config, new InstagramParser())
        {
        }

        public InstagramDataProvider(InstagramDataConfig config, IParser<InstagramSchema> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<InstagramSchema>> LoadDataAsync()
        {
            InternetRequestSettings settings = new InternetRequestSettings()
            {
                RequestedUri = this.GetApiUrl()
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

        private Uri GetApiUrl()
        {
            if (_config.QueryType == "tag")
            {
                return new Uri(string.Format(URL, _config.Query, _config.Tokens["ClientId"]));
            }
            else
            {
                return new Uri(string.Format(URLUserID, _config.Query, _config.Tokens["ClientId"]));
            }
        }
    }
}
