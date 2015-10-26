using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.InternetClient;
using AppStudio.DataProviders.Rss.Parser;

namespace AppStudio.DataProviders.Rss
{
    public class RssDataProvider : DataProviderBase<RssDataConfig, RssSchema>
    {
        public RssDataProvider(RssDataConfig config)
            : base(config, new RssParser())
        {
        }

        public RssDataProvider(RssDataConfig config, IParser<RssSchema> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<RssSchema>> LoadDataAsync()
        {
            InternetRequestSettings settings = new InternetRequestSettings()
            {
                RequestedUri = new Uri(_config.Url)
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
