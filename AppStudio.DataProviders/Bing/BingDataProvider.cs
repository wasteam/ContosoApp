using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Bing.Parser;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.InternetClient;

namespace AppStudio.DataProviders.Bing
{
    public class BingDataProvider : DataProviderBase<BingDataConfig, BingSchema>
    {
        public BingDataProvider(BingDataConfig config)
            : base(config, new BingParser())
        {
        }

        public BingDataProvider(BingDataConfig config, IParser<BingSchema> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<BingSchema>> LoadDataAsync()
        {
            InternetRequestSettings settings = new InternetRequestSettings()
            {
                RequestedUri = new Uri(string.Format("http://www.bing.com/search?q={0}&loc:{1}&format=rss", WebUtility.UrlEncode(_config.Query), _config.QueryType))
            };

            var result = await InternetRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return _parser.Parse(result.Result);
            }

            throw new RequestFailedException();
        }
    }
}
