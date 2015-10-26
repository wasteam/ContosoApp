using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core.Parser;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.InternetClient;

namespace AppStudio.DataProviders.DynamicStorage
{
    public class DynamicStorageDataProvider<T> : DataProviderBase<DynamicStorageDataConfig, T> where T : SchemaBase
    {
        public DynamicStorageDataProvider(DynamicStorageDataConfig config)
            : base(config, new GenericParser<T>())
        {
        }

        public DynamicStorageDataProvider(DynamicStorageDataConfig config, IParser<T> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<T>> LoadDataAsync()
        {
            var result = await InternetRequest.DownloadAsync(this.CreateSettings());
            if (result.Success)
            {
                return _parser.Parse(result.Result);
            }

            throw new RequestFailedException();
        }

        private InternetRequestSettings CreateSettings()
        {
            string url = string.Format("{0}&pageIndex={1}&blockSize={2}", _config.Url, _config.PageIndex, _config.BlockSize);
            InternetRequestSettings settings = new InternetRequestSettings
            {
                RequestedUri = new Uri(url),
                UserAgent = "NativeHost"
            };

            settings.Headers["WAS-APPID"] = _config.AppId;
            settings.Headers["WAS-STOREID"] = _config.StoreId;
            settings.Headers["WAS-DEVICETYPE"] = _config.DeviceType;
            settings.Headers["WAS-ISBACKGROUND"] = _config.IsBackgroundTask.ToString();
            return settings;
        }
    }
}
