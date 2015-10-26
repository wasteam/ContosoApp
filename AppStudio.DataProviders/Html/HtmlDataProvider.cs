using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppStudio.DataProviders.Html.Parser;
using AppStudio.DataProviders.LocalStorage;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStudio.DataProviders.Html
{
    public class HtmlDataProvider : DataProviderBase<LocalStorageDataConfig, HtmlSchema>
    {
        public HtmlDataProvider(LocalStorageDataConfig config)
            : base(config, new HtmlParser())
        {
        }

        public HtmlDataProvider(LocalStorageDataConfig config, IParser<HtmlSchema> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<HtmlSchema>> LoadDataAsync()
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
