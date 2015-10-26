using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Flickr.Parser;
using AppStudio.DataProviders.InternetClient;

namespace AppStudio.DataProviders.Flickr
{
    public class FlickrDataProvider : DataProviderBase<FlickrDataConfig, FlickrSchema>
    {
        public FlickrDataProvider(FlickrDataConfig config)
            : base(config, new FlickrParser())
        {
        }

        public FlickrDataProvider(FlickrDataConfig config, IParser<FlickrSchema> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<FlickrSchema>> LoadDataAsync()
        {
            InternetRequestSettings settings = new InternetRequestSettings()
            {
                RequestedUri = new Uri(string.Format("http://api.flickr.com/services/feeds/photos_public.gne?{0}={1}", _config.QueryType, WebUtility.UrlEncode(_config.Query)))
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
