using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.InternetClient;
using AppStudio.DataProviders.TouchDevelop.Parser;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStudio.DataProviders.TouchDevelop
{
    public class TouchDevelopDataProvider : DataProviderBase<TouchDevelopDataConfig, TouchDevelopSchema>
    {
        public TouchDevelopDataProvider(TouchDevelopDataConfig config)
            : base(config, new TouchDevelopParser())
        {
        }

        public TouchDevelopDataProvider(TouchDevelopDataConfig config, IParser<TouchDevelopSchema> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<TouchDevelopSchema>> LoadDataAsync()
        {
            try
            {
                TouchDevelopSchema record = await GetRemoteProject();
                if (record == null)
                {
                    record = await GetLocalProject();
                }

                return new List<TouchDevelopSchema>() { record };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new RequestFailedException();
            }
        }

        private async Task<TouchDevelopSchema> GetRemoteProject()
        {
            InternetRequestSettings settings = new InternetRequestSettings()
            {
                RequestedUri = new Uri(string.Format("https://www.touchdevelop.com/api/{0}", _config.ScriptId))
            };

            var result = await InternetRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return _parser.Parse(result.Result).FirstOrDefault();
            }
            return null;
        }

        private async Task<TouchDevelopSchema> GetLocalProject()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(string.Format("ms-appx://{0}", _config.LocalDataSource)));
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                string data = await r.ReadToEndAsync();
                return JsonConvert.DeserializeObject<TouchDevelopSerialized>(data).Items.FirstOrDefault();
            }
        }
    }
}
