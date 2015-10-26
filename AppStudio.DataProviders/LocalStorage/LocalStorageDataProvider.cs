using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core.Parser;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStudio.DataProviders.LocalStorage
{
    public class LocalStorageDataProvider<T> : DataProviderBase<LocalStorageDataConfig, T> where T : SchemaBase
    {
        public LocalStorageDataProvider(LocalStorageDataConfig config)
            : base(config, new GenericParser<T>())
        {
        }

        public LocalStorageDataProvider(LocalStorageDataConfig config, IParser<T> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<T>> LoadDataAsync()
        {
            var uri = new Uri(string.Format("ms-appx://{0}", _config.FilePath));

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return _parser.Parse(await r.ReadToEndAsync());
            }
        }

        public override bool IsLocal
        {
            get
            {
                return true;
            }
        }
    }
}
